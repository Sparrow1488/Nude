using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Exceptions;
using Nude.Models.Requests;
using Nude.Providers;

namespace Nude.API.Services.Parsing;

public class MangaParsingService : IMangaParsingService
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;
    private readonly IMapper _mapper;

    public MangaParsingService(
        AppDbContext context, 
        INudeParser parser,
        IMapper mapper)
    {
        _context = context;
        _parser = parser;
        _mapper = mapper;
    }
    
    public async Task<ParsingResponse> CreateRequestAsync(string mangaUrl)
    {
        var exMangaId = _parser.Helper.GetIdFromUrl(mangaUrl);
        var request = new ParsingRequest
        {
            Url = mangaUrl,
            Status = Status.Failed
        };
        
        // Check in Database
        var isExists = await _context.Mangas.AnyAsync(x => x.ExternalId == exMangaId);
        if (isExists)
        {
            request.Status = Status.Success;
            var response = _mapper.Map<ParsingResponse>(request);
            return response;
        }

        // Get from external source
        var externalExists = await _parser.ExistsAsync(mangaUrl);
        if (!externalExists)
        {
            throw new NotFoundException("External manga not found", exMangaId, "Manga");
        }

        request.UniqueId = Guid.NewGuid().ToString();
        request.Status = Status.Processing;        
        await _context.ParsingRequests.AddAsync(request);
        await _context.SaveChangesAsync();

        return _mapper.Map<ParsingResponse>(request);
    }

    public async Task<ParsingResponse> GetRequestAsync(string uniqueId)
    {
        var request = await _context.ParsingRequests
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId)
            ?? throw new NotFoundException("Request not found", uniqueId, "ParsingRequest");

        return _mapper.Map<ParsingResponse>(request);
    }
}