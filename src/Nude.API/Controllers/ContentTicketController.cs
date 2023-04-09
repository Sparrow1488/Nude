using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets;

namespace Nude.API.Controllers;

[ApiController, Route("content-tickets")]
public class ContentTicketController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IContentTicketService _service;

    public ContentTicketController(
        IMapper mapper,
        IContentTicketService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContentTicketRequest request)
    {
        var ticket = await _service.FindSimilarAsync(request.SourceUrl)
            ?? await _service.CreateAsync(request.SourceUrl);

        return Ok(_mapper.Map<ContentTicketResponse>(ticket));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);
        return Ok(_mapper.Map<ContentTicketResponse>(ticket));
    }
}