using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.API.Models.Formats;

#region Rider annotations

// ReSharper disable once InvertIf

#endregion

namespace Nude.API.Services.Formatters;

public class ContentFormatterService : IContentFormatterService
{
    private readonly ITelegraphClient _telegraph;
    private readonly ILogger<ContentFormatterService> _logger;

    public ContentFormatterService(
        ITelegraphClient telegraph,
        ILogger<ContentFormatterService> logger)
    {
        _telegraph = telegraph;
        _logger = logger;
    }

    public async Task<FormattedContent> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images, 
        FormatType type)
    {
        var imagesList = images.ToList();
        
        if (type == FormatType.Telegraph)
        {
            var tghImages = await UploadTelegraphImagesAsync(imagesList);
            var pageUrl = await _telegraph.CreatePageAsync(title, text, tghImages);

            return new TelegraphContent { Url = pageUrl };
        }

        throw new NotImplementedException();
    }

    private async Task<List<string>> UploadTelegraphImagesAsync(IEnumerable<string> exImages)
    {
        var images = exImages.ToList();
        var convertedImages = new List<string>();

        var totalImages = images.Count;
        
        _logger.LogInformation("Starting upload {total} images to tgh", totalImages);

        for (var i = 0; i < images.Count; i++)
        {
            var tghImage = await _telegraph.UploadFileAsync(images[i]);
            convertedImages.Add(tghImage);

            var currentImage = i + 1;
            if (currentImage % 5 == 0 || i == images.Count - 1)
            { 
                _logger.LogInformation(
                    "({current}/{total}) Uploading images to telegraph",
                    currentImage,
                    totalImages);
            }
        }
        
        _logger.LogInformation("Images uploaded success");

        return convertedImages;
    }
}