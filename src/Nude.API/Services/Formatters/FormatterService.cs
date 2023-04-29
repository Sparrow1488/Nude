using System.Collections;
using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.API.Models.Formats;
using Nude.API.Services.Formatters.Variables;

#region Rider annotations

// ReSharper disable once InvertIf

#endregion

namespace Nude.API.Services.Formatters;

public class FormatterService : IFormatterService
{
    private readonly ITelegraphClient _telegraph;
    private readonly ILogger<FormatterService> _logger;

    public FormatterService(
        ITelegraphClient telegraph,
        ILogger<FormatterService> logger)
    {
        _telegraph = telegraph;
        _logger = logger;
    }

    public event Func<IDictionary, Task>? FormatProgressUpdated;

    public async Task<Format> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images, 
        FormatType type)
    {
        var imagesList = images.ToList();
        
        // TODO: Format Handlers
        if (type == FormatType.Telegraph)
        {
            var tghImages = await UploadTelegraphImagesAsync(imagesList);
            var pageUrl = await _telegraph.CreatePageAsync(title, text, tghImages);

            return new TelegraphFormat { Url = pageUrl };
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
            if (string.IsNullOrWhiteSpace(tghImage)) continue;
            
            convertedImages.Add(tghImage);

            var currentImage = i + 1;
            LogUploadingProgress(totalImages, currentImage);
            
            if (FormatProgressUpdated != null)
                await FormatProgressUpdated.Invoke(CreateProgressVariables(totalImages, currentImage));
        }
        
        _logger.LogInformation("Images uploaded success");

        return convertedImages;
    }

    private void LogUploadingProgress(int totalImages, int currentImage)
    {
        if (currentImage % 5 == 0 || currentImage == totalImages)
        { 
            _logger.LogInformation(
                "({current}/{total}) Uploading images to telegraph",
                currentImage,
                totalImages);
        }
    }

    private static IDictionary CreateProgressVariables(
        int totalImages,
        int currentImage)
    {
        return new Dictionary<string, object>
        {
            { FormattingVariables.TotalImages, totalImages },
            { FormattingVariables.CurrentImageProcessing, currentImage }
        };
    }
}