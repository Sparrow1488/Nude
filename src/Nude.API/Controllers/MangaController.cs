using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Blacklists;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Users;
using Nude.API.Services.Blacklists;
using Nude.API.Services.Mangas;
using Nude.API.Services.Users;
using Nude.API.Services.Views;

namespace Nude.API.Controllers;

[Route("manga")]
public class MangaController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUserSession _userSession;
    private readonly IViewService _viewService;
    private readonly IMangaService _service;
    private readonly IBlacklistService _blacklistService;

    public MangaController(
        IMapper mapper,
        IUserSession userSession,
        IViewService viewService,
        IMangaService service,
        IBlacklistService blacklistService)
    {
        _mapper = mapper;
        _userSession = userSession;
        _viewService = viewService;
        _service = service;
        _blacklistService = blacklistService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _service.GetByIdAsync(id);

        if (manga != null)
        {
            if (_userSession.IsAuthorized())
                await _viewService.CreateViewAsync(await _userSession.GetUserAsync(), manga);
            
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException(id));
    }
    
    [HttpGet("random")]
    public async Task<IActionResult> GetRandom(FormatType? format)
    {
        User? user = null;
        var viewedIds = Array.Empty<int>();
        Blacklist? blacklist = null;
        
        if (_userSession.IsAuthorized())
        {
            user = await _userSession.GetUserAsync();
            viewedIds = await GetViewedIdsAsync(user);
            blacklist = await _blacklistService.GetAsync(user);
        }
        
        var filter = new SearchMangaFilter
        {
            Format = format,
            ExceptIds = viewedIds
        };
        
        var manga = await _service.GetRandomAsync(filter, blacklist);

        if (manga != null)
        {
            if (user is not null)
                await _viewService.CreateViewAsync(user, manga);
            
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException());
    }

    private async Task<int[]> GetViewedIdsAsync(User user)
    {
        var viewedIds = (await _viewService
            .FindByAsync(x => x.Manga != null && x.User.Id == user.Id))
            .Select(x => x.Manga!.Id);

        var allIds = await _service.GetAllAsync();

        if (viewedIds.Count() == allIds.Length)
        {
            viewedIds = Array.Empty<int>();
        }

        return viewedIds.ToArray();
    }

    [HttpGet]
    public async Task<IActionResult> FindBy(string? sourceUrl, string? contentKey, FormatType? format)
    {
        MangaEntry? manga = null;
        
        if(!string.IsNullOrWhiteSpace(sourceUrl))
            manga = await _service.FindBySourceUrlAsync(sourceUrl, format);
        if(!string.IsNullOrWhiteSpace(contentKey))
            manga = await _service.FindByContentKeyAsync(contentKey, format);

        if (manga != null)
        {
            if (_userSession.IsAuthorized())
                await _viewService.CreateViewAsync(await _userSession.GetUserAsync(), manga);
            
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException());
    }

    private static NotFoundException NotFoundException(int? mangaId = null)
    {
        return new NotFoundException(
            "Manga not found",
            mangaId?.ToString(),
            nameof(MangaEntry)
        );
    }
}