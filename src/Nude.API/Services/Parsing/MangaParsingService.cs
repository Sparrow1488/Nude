using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.Models.Requests;
using Nude.Providers;

namespace Nude.API.Services.Parsing;

public class MangaParsingService : IMangaParsingService
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;

    public MangaParsingService(AppDbContext context, INudeParser parser)
    {
        _context = context;
        _parser = parser;
    }
    
    public async Task<ParsingResponse> CreateRequestAsync(string mangaUrl)
    {
        // Check in Database
        var dbExists = await _context.Mangas.FirstOrDefaultAsync(x => x.OriginUrl.Value == mangaUrl);
        if (dbExists is not null)
        {
            return new ParsingResponse
            {
                UniqueId = Guid.NewGuid().ToString(),
                MangaUrl = mangaUrl,
                IsAlreadyExists = true,
                Status = Status.Success.ToString()
            };
        }

        // Get from external source
        var externalExists = await _parser.ExistsAsync(mangaUrl);
        if (!externalExists)
        {
            throw new Exception("Not found exception");
        }
        
        var uniqueId = Guid.NewGuid().ToString();
        await _context.ParsingRequests.AddAsync(new ParsingRequest
        {
            UniqueId = uniqueId,
            Url = mangaUrl
        });
        await _context.SaveChangesAsync();

        return new ParsingResponse
        {
            UniqueId = uniqueId,
            MangaUrl = mangaUrl,
            Status = Status.Processing.ToString()
        };
    }
}