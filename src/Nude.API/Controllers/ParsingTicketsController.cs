// using Microsoft.AspNetCore.Mvc;
// using Nude.API.Contracts.Parsing.Requests;
// using Nude.API.Contracts.Parsing.Responses;
// using Nude.API.Services.Parsing;
//
// namespace Nude.API.Controllers;
//
// [ApiController, Route("parsing/tickets")]
// public class ParsingTicketsController : ControllerBase
// {
//     private readonly IParsingTicketsService _ticketsService;
//
//     public ParsingTicketsController(IParsingTicketsService ticketsService)
//     {
//         _ticketsService = ticketsService;
//     }
//
//     [HttpGet("{id}")]
//     public Task<ParsingResponse> GetTicketById(int id)
//         => _ticketsService.GetTicketAsync(id);
//
//     [HttpPost]
//     public Task<ParsingResponse> CreateTicket(ParsingCreateRequest request)
//         => _ticketsService.CreateTicketAsync(request);
// }