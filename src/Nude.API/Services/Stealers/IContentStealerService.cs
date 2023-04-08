using Nude.API.Services.Stealers.Results;

namespace Nude.API.Services.Stealers;

public interface IContentStealerService
{
    Task<ContentStealingResult> StealContentAsync(string sourceUrl);
}