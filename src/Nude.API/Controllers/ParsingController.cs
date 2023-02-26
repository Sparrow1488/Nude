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


    [HttpPost("requests/new")]
    public Task<ParsingResponse> CreateRequests(string mangaUrl)
        => _service.CreateRequestAsync(mangaUrl);
}