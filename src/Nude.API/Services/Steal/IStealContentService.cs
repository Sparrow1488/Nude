using Nude.API.Services.Steal.Results;

namespace Nude.API.Services.Steal;

public interface IStealContentService
{
    Task<ContentStealingResult> StealContentAsync(string sourceUrl);
}