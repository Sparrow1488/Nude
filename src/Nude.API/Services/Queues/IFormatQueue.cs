using Nude.API.Services.Queues.Models;

namespace Nude.API.Services.Queues;

public interface IFormatQueue
{
    Task<FormatModelAgent?> DequeueAsync();
}