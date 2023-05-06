using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Contracts.Tags.Responses;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Managers;
using Nude.API.Services.Blacklists;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("users"), Authorize]
public class UserController : ApiController
{
    private readonly IBlacklistService _blacklistService;
    private readonly IMapper _mapper;
    private readonly IUserSession _session;
    private readonly ITagManager _tagManager;
    private readonly IContentTicketService _ticketService;

    public UserController(
        IBlacklistService blacklistService,
        IMapper mapper,
        IUserSession session,
        ITagManager tagManager,
        IContentTicketService ticketService)
    {
        _blacklistService = blacklistService;
        _mapper = mapper;
        _session = session;
        _tagManager = tagManager;
        _ticketService = ticketService;
    }

    [HttpGet("me/tickets")]
    public async Task<IActionResult> GetUserActiveTickets()
    {
        var user = await _session.GetUserAsync();
        var tickets = await _ticketService.GetUserTicketsAsync(user.Id);

        return Ok(_mapper.Map<ContentTicketResponse[]>(tickets));
    }

    [HttpPost("me/blacklist/reset")]
    public async Task<IActionResult> SetDefaultBlacklist()
    {
        var user = await _session.GetUserAsync();
        var defaultBlacklist = await _blacklistService.GetDefaultAsync();

        if (user.Blacklist is not null)
        {
            await _blacklistService.RemoveAsync(user.Blacklist);
        }

        var blacklist = await _blacklistService.CreateAsync(user, defaultBlacklist.Tags);
        return Ok(_mapper.Map<BlacklistResponse>(blacklist));
    }

    [HttpGet("me/blacklist")]
    public async Task<IActionResult> GetBlacklist()
    {
        var user = await _session.GetUserAsync();
        var blacklist = await _blacklistService.GetAsync(user);

        if (blacklist is not null)
        {
            return Ok(_mapper.Map<BlacklistResponse>(blacklist));
        }
        
        var exception = new NotFoundException("Blacklist not found");
        return Exception(exception);
    }

    [HttpPut("me/blacklist/tags")]
    public async Task<IActionResult> UpdateBlacklistTags(string tags)
    {
        var user = await _session.GetUserAsync();
        
        if (user.Blacklist is null)
        {
            var defaultBlacklist = await _blacklistService.GetDefaultAsync();
            user.Blacklist = await _blacklistService.CreateAsync(user, defaultBlacklist.Tags);
        }

        var tagsList = SplitTagsFromQuery(tags);

        var dbTags = await _tagManager.FindAsync(tagsList);
        await _blacklistService.AddTagsAsync(user.Blacklist, dbTags);

        return Ok(_mapper.Map<TagResponse[]>(dbTags));
    }
    
    [HttpDelete("me/blacklist/tags")]
    public async Task<IActionResult> DeleteBlacklistTags(string tags)
    {
        var user = await _session.GetUserAsync();
        
        if (user.Blacklist is null)
        {
            return Ok();
        }
        
        var blacklist = await _blacklistService.GetAsync(user);

        var tagsList = SplitTagsFromQuery(tags);
        var dbTags = await _tagManager.FindAsync(tagsList);
        
        await _blacklistService.RemoveTagsAsync(blacklist!, dbTags);

        return Ok(_mapper.Map<TagResponse[]>(dbTags));
    }

    private static string[] SplitTagsFromQuery(string queryTags) =>
        queryTags.Split("-").Select(x => x.Trim()).ToArray();
}