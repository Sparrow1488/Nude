using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;

namespace Nude.API.Services.Parsing;

public interface IParsingTicketsService
{
    Task<ParsingResponse> CreateTicketAsync(ParsingCreateRequest request);
    Task<ParsingResponse> GetRequestAsync(string uniqueId);
}