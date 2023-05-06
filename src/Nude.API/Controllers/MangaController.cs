using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
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

    public MangaController(
        IMapper mapper,
        IUserSession userSession,
        IViewService viewService,
        IMangaService service)
    {
        _mapper = mapper;
        _userSession = userSession;
        _viewService = viewService;
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _service.GetByIdAsync(id);

        if (manga != null)
        {
            await _viewService.CreateViewAsync(await _userSession.GetUserAsync(), manga);
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException(id));
    }
    
    [HttpGet("random")]
    public async Task<IActionResult> GetRandom(FormatType? format)
    {
        var manga = await _service.GetRandomAsync(format);

        if (manga != null)
        {
            await _viewService.CreateViewAsync(await _userSession.GetUserAsync(), manga);
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException());
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