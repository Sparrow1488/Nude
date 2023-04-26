using Nude.API.Models.Formats;

namespace Nude.API.Services.Queues.Models;

public class FormatModelAgent
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ContentKey { get; set; } = null!;
    public ICollection<string> Images { get; set; } = null!;
    public FormatType Type { get; set; }
    public Func<Format, Task> ReleaseFunc { get; set; } = null!;

    public Task ReleaseAsync(Format format)
    {
        return ReleaseFunc.Invoke(format);
    }
}