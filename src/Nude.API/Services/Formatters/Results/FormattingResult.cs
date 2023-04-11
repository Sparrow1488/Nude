using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters.Results;

public class FormattingResult : IServiceResult<Format>
{
    public bool IsSuccess { get; set; }
    public Format? Result { get; set; }
    public Exception? Exception { get; set; }
}