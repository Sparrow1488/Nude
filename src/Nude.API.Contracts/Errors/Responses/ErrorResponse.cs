using System.Collections;

namespace Nude.API.Contracts.Errors.Responses;

public struct ErrorResponse
{
    public string Message { get; set; }
    public string? Exception { get; set; }
    public IDictionary? Data { get; set; }
    public int Status { get; set; }
}