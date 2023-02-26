using Nude.API.Contracts.Parsing.Responses;

namespace Nude.API.Services.Parsing;

public interface IMangaParsingService
{
    Task<ParsingResponse> CreateRequestAsync(string mangaUrl);
    Task<ParsingResponse> GetRequestAsync(string uniqueId);
}