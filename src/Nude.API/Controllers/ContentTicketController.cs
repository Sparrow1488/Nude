using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets;

namespace Nude.API.Controllers;

[Route($"{ApiDefaults.CurrentVersion}/content-tickets")]
public class ContentTicketController : ApiController
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
        var result = await _service.CreateAsync(request.SourceUrl);

        if (result.IsSuccess)
        {
            return Ok(_mapper.Map<ContentTicketResponse>(result.Result));
        }

        return Exception(result.Exception!);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);
        
        if (ticket != null)
        {
            return Ok(_mapper.Map<ContentTicketResponse>(ticket));
        }

        return Exception(new NotFoundException(
            "Ticket not found",
            id.ToString(),
            nameof(ContentTicket))
        );
    }
}