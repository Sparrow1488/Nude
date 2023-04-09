using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Tickets;
using Nude.API.Services.Subscribers;
using Nude.API.Services.Tickets;

namespace Nude.API.Controllers;

[ApiController, Route("format-tickets")]
public class FormatTicketController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IContentFormatTicketService _service;
    private readonly ISubscribersService _subscribersService;

    public FormatTicketController(
        IMapper mapper,
        IContentFormatTicketService service,
        ISubscribersService subscribersService)
    {
        _mapper = mapper;
        _service = service;
        _subscribersService = subscribersService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(FormatTicketRequest request)
    {
        var creationResult = await _service.CreateAsync(
            request.EntryId,
            request.EntryType,
            request.FormatType
        );

        if (creationResult.IsSuccess)
        {
            await SubscribeAsync(creationResult.FormatTicket!, request.CallbackUrl);
            return Ok(_mapper.Map<FormatTicketResponse>(creationResult.FormatTicket));
        }

        return BadRequest(creationResult.Exception);
    }
    
    private Task SubscribeAsync(ContentFormatTicket ticket, string? callback)
    {
        return !string.IsNullOrWhiteSpace(callback) 
            ? _subscribersService.CreateAsync(ticket.Id.ToString(), nameof(ContentFormatTicket), callback) 
            : Task.CompletedTask;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);

        if (ticket != null)
        {
            return Ok(_mapper.Map<FormatTicketResponse>(ticket));
        }

        return NotFound();
    }
}