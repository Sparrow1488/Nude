using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("content-tickets")]
public class ContentTicketController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUserSession _session;
    private readonly IContentTicketService _service;

    public ContentTicketController(
        IMapper mapper,
        IUserSession session,
        IContentTicketService service)
    {
        _mapper = mapper;
        _session = session;
        _service = service;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Create(ContentTicketRequest request)
    {
        var user = await _session.GetUserAsync();
        var userTickets = await _service.GetUserTicketsAsync(user.Id);

        if (userTickets.Count >= 2)
        {
            var exception = new TicketLimitExceededException("You have more than 2 tickets");
            return Exception(exception);
        }
        
        var result = await _service.CreateAsync(request.SourceUrl, user);

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