using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("users")]
public class UserController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUserSession _session;
    private readonly IContentTicketService _ticketService;

    public UserController(
        IMapper mapper,
        IUserSession session,
        IContentTicketService ticketService)
    {
        _mapper = mapper;
        _session = session;
        _ticketService = ticketService;
    }

    [HttpGet("me/tickets"), Authorize]
    public async Task<IActionResult> GetUserActiveTickets()
    {
        var user = await _session.GetUserAsync();
        var tickets = await _ticketService.GetUserTicketsAsync(user.Id);

        return Ok(_mapper.Map<ContentTicketResponse[]>(tickets));
    }
}