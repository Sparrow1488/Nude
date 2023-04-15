using AutoMapper;
using BooruSharp.Booru;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Images;
using Nude.API.Services.Images;
using Nude.API.Services.Images.Models;

namespace Nude.API.Controllers;

[Route("v2/images")]
public class ImageController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IImagesService _service;

    public ImageController(
        IMapper mapper,
        IImagesService service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var image = await _service.GetByIdAsync(id);

        if (image != null)
        {
            return Ok(_mapper.Map<ImageResponse>(image));
        }

        return Exception(new NotFoundException(
            "Image not found",
            id.ToString(),
            nameof(ImageEntry))
        );
    }

    [HttpGet("booru")]
    public async Task<IActionResult> FindBooruByTags(string tags)
    {
        var parser = new Gelbooru();

        var creationModels = new List<ImageCreationModel>();
        var images = new List<ImageEntry>();
        var posts = await parser.GetRandomPostsAsync(5, tags);
        
        foreach (var post in posts)
        {
            var imageUrl = post.FileUrl.AbsoluteUri;
            var contentKey = ContentKeyGenerator.Generate(nameof(ImageEntry), imageUrl);
            
            if (!await _service.ExistsAsync(contentKey))
            {
                var model = new ImageCreationModel
                {
                    Url = imageUrl,
                    ContentKey = contentKey,
                    Tags = post.Tags,
                    ExternalSourceId = post.ID.ToString(),
                    ExternalSourceUrl = post.PostUrl.AbsoluteUri
                };

                creationModels.Add(model);
                continue;
            }

            var exists = await _service.GetByContentKeyAsync(contentKey);
            images.Add(exists!);
        }

        var result = await _service.CreateRangeAsync(creationModels);
        images.AddRange(result.Result!);

        return Ok(_mapper.Map<ImageResponse[]>(images));
    }
}