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
    private readonly IUsersService _usersService;
    private readonly IContentTicketService _ticketService;

    public UserController(
        IMapper mapper,
        IUsersService usersService,
        IContentTicketService ticketService)
    {
        _mapper = mapper;
        _usersService = usersService;
        _ticketService = ticketService;
    }

    [HttpGet("me/tickets"), Authorize]
    public async Task<IActionResult> GetUserActiveTickets()
    {
        var userId = int.Parse(HttpContext.User.FindFirst("sub")!.Value);
        var tickets = await _ticketService.GetUserTicketsAsync(userId);

        return Ok(_mapper.Map<ContentTicketResponse[]>(tickets));
    }
}