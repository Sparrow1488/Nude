using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Models.Formats;
using Nude.API.Services.Mangas;

namespace Nude.API.Controllers;

[ApiController, Route("manga")]
public class MangaController : ControllerBase
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

        return NotFound();
    }
    
    [HttpGet("random")]
    public async Task<IActionResult> GetRandom(FormatType? format)
    {
        var manga = await _service.GetRandomAsync(format);

        if (manga != null)
        {
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> FindBySourceUrl(string sourceUrl, FormatType? format)
    {
        var manga = await _service.FindBySourceUrlAsync(sourceUrl, format);

        if (manga != null)
        {
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return NotFound();
    }
}