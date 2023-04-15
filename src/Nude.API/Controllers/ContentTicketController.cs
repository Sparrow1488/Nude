using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("content-tickets")]
public class ContentTicketController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;
    private readonly IContentTicketService _service;

    public ContentTicketController(
        IMapper mapper,
        IUsersService usersService,
        IContentTicketService service)
    {
        _mapper = mapper;
        _usersService = usersService;
        _service = service;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Create(ContentTicketRequest request)
    {
        var userId = int.Parse(HttpContext.User.FindFirst("sub")!.Value);
        var user = await _usersService.GetByIdAsync(userId);
        
        var userTickets = await _service.GetUserTicketsAsync(user!.Id);

        if (userTickets.Count >= 3)
        {
            var exception = new TicketLimitExceededException("You have more than 3 tickets");
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