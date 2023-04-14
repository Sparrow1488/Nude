using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas;

namespace Nude.API.Controllers;

[Route($"{ApiDefaults.CurrentVersion}/manga")]
public class MangaController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMangaService _service;

    public MangaController(
        IMapper mapper,
        IMangaService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _service.GetByIdAsync(id);

        if (manga != null)
        {
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
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return Exception(NotFoundException());
    }

    [HttpGet]
    public async Task<IActionResult> FindBy(string? sourceUrl, string? contentKey, FormatType? format)
    {
        MangaEntry? mangaEntry = null;
        
        if(!string.IsNullOrWhiteSpace(sourceUrl))
            mangaEntry = await _service.FindBySourceUrlAsync(sourceUrl, format);
        if(!string.IsNullOrWhiteSpace(contentKey))
            mangaEntry = await _service.FindByContentKeyAsync(contentKey, format);

        if (mangaEntry != null)
        {
            return Ok(_mapper.Map<MangaResponse>(mangaEntry));
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