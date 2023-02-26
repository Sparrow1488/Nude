using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Services.Parsing;

namespace Nude.API.Controllers;

[ApiController, Route("parsing")]
public class ParsingController : ControllerBase
{
    private readonly IMangaParsingService _service;

    public ParsingController(IMangaParsingService service)
    {
        _service = service;
    }

    [HttpGet("requests/{uniqueId}")]
    public Task<ParsingResponse> GetRequestByUniqueId(string uniqueId)
        => _service.GetRequestAsync(uniqueId);

    [HttpPost("requests/new")]
    public Task<ParsingResponse> CreateRequest(string mangaUrl)
        => _service.CreateRequestAsync(mangaUrl);
}