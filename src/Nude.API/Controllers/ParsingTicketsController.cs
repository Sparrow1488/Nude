using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Services.Parsing;

namespace Nude.API.Controllers;

[ApiController, Route("parsing/tickets")]
public class ParsingTicketsController : ControllerBase
{
    private readonly IParsingTicketsService _ticketsService;

    public ParsingTicketsController(IParsingTicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }

    [HttpGet("{uniqueId}")]
    public Task<ParsingResponse> GetTicketByUniqueId(string uniqueId)
        => _ticketsService.GetTicketAsync(uniqueId);

    [HttpPost]
    public Task<ParsingResponse> CreateTicket(ParsingCreateRequest request)
        => _ticketsService.CreateTicketAsync(request);
}