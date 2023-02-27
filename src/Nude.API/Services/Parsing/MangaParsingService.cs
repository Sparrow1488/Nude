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

        #region Check manga exists

        var isMangaExists = await _context.Mangas.AnyAsync(x => x.ExternalId == exMangaId);
        if (isMangaExists)
        {
            request.Status = Status.Success;
            var response = _mapper.Map<ParsingResponse>(request);
            return response;
        }

        #endregion

        #region Check similar requests

        var isSimilarRequestExists = await _context.ParsingRequests
            .AnyAsync(x => x.ExternalId == exMangaId);
        if (isSimilarRequestExists)
        {
            request.Status = Status.Processing;
            request.Message = "Similar request waiting for processing";
            var response = _mapper.Map<ParsingResponse>(request);
            return response;
        }

        #endregion

        #region Save request

        request.UniqueId = Guid.NewGuid().ToString();
        request.Status = Status.Processing;
        request.ExternalId = exMangaId;
        await _context.AddAsync(request);
        await _context.SaveChangesAsync();

        #endregion
        
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