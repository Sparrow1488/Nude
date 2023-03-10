using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Services.Manga;

namespace Nude.API.Controllers;

[ApiController, Route("manga")]
public class MangaController : ControllerBase
{
    private readonly IMangaService _service;

    public MangaController(IMangaService service)
    {
        _service = service;
    }

    // TODO: IRuntimeQueryableFilter
    [HttpGet("{id}")]
    public Task<MangaResponse> GetById(int id)
        => _service.GetByIdAsync(id);
    //
    // [HttpGet("manga")]
    // public Task<MangaResponse> GetByExternalId(string externalId)
    //     => _service.GetByExternalIdAsync(externalId);
    
    [HttpGet]
    public Task<MangaResponse> GetByUrl(string url)
        => _service.GetByUrlAsync(url);
}