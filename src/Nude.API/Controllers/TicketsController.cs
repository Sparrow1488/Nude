using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Tickets;
using Nude.API.Services.Tickets;

namespace Nude.API.Controllers;

[ApiController, Route("tickets")]
public class TicketsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IContentTicketService _service;

    public TicketsController(
        IMapper mapper,
        IContentTicketService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReceiveContentTicketRequest request)
    {
        var ticket = await _service.CreateAsync(request.SourceUrl);
        return Ok(_mapper.Map<ContentTicketResponse>(ticket));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);
        return Ok(_mapper.Map<ContentTicketResponse>(ticket));
    }
}