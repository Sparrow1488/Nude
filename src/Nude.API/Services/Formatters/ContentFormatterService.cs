using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters;

public class ContentFormatterService : IContentFormatterService
{
    private readonly ITelegraphClient _telegraph;

    public ContentFormatterService(ITelegraphClient telegraph)
    {
        _telegraph = telegraph;
    }

    public Task<FormattedContent> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images, 
        FormatType type)
    {
        if (type == FormatType.Telegraph)
        {
            
        }

        throw new NotImplementedException();
    }

    // private async Task<string> UploadImageAsync()
    // {
    //     
    // }
}