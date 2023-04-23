using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Tickets;
using Nude.API.Services.Limits;
using Nude.API.Services.Limits.Handlers;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("content-tickets")]
public class ContentTicketController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUserSession _session;
    private readonly ILimitService _limitService;
    private readonly IContentTicketService _service;

    public ContentTicketController(
        IMapper mapper,
        IUserSession session,
        ILimitService limitService,
        IContentTicketService service)
    {
        _mapper = mapper;
        _session = session;
        _limitService = limitService;
        _service = service;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Create(ContentTicketRequest request)
    {
        var user = await _session.GetUserAsync();

        var limitResult = await _limitService.IsLimitedAsync(LimitTarget.ContentTicketCreation);
        if (!limitResult.IsSuccess)
            return Exception(limitResult.Exception!);
        
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