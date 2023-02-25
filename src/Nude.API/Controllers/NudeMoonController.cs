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

    [HttpGet("manga")]
    public Task<MangaResponse> GetByUrl(string url)
        => _service.GetByUrlAsync(url);
}