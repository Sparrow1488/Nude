using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Services.Tickets;

namespace Nude.API.Controllers;

[ApiController, Route("format-tickets")]
public class FormatTicketController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IContentFormatTicketService _service;

    public FormatTicketController(
        IMapper mapper,
        IContentFormatTicketService service)
    {
        _mapper = mapper;
        _service = service;
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
            return Ok(_mapper.Map<FormatTicketResponse>(creationResult.FormatTicket));
        }

        return BadRequest(creationResult.Exception);
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