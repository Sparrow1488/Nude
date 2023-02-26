using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Services.Manga;

namespace Nude.API.Controllers;

[ApiController, Route("nude-moon")]
public class NudeMoonController : ControllerBase
{
    private readonly IMangaService _service;

    public NudeMoonController(IMangaService service)
    {
        _service = service;
    }

    // TODO: IRuntimeQueryableFilter
    // [HttpGet("manga")]
    // public Task<MangaResponse> GetById(int id)
    //     => _service.GetByIdAsync(id);
    //
    // [HttpGet("manga")]
    // public Task<MangaResponse> GetByExternalId(string externalId)
    //     => _service.GetByExternalIdAsync(externalId);
    
    [HttpGet("manga")]
    public Task<MangaResponse> GetByUrl(string url)
        => _service.GetByUrlAsync(url);
}