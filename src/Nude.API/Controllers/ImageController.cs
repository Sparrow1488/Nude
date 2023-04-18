using AutoMapper;
using BooruSharp.Booru;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Services.Storages;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Images;
using Nude.API.Services.Images;
using Nude.API.Services.Images.Models;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("images")]
public class ImageController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IUserSession _session;
    private readonly IFileStorage _storage;
    private readonly IImagesService _service;

    public ImageController(
        IMapper mapper,
        IUserSession session,
        IFileStorage storage,
        IImagesService service)
    {
        _mapper = mapper;
        _session = session;
        _storage = storage;
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

    [HttpPost("new"), Authorize]
    public async Task<IActionResult> Create(IFormFile file)
    {
        if (!file.ContentType.Contains("image"))
        {
            var exception = new BadRequestException(
                $"Current file type ({file.ContentType}) not supported"
            );
            return Exception(exception);
        }

        await using var fileStream = file.OpenReadStream();
        fileStream.Seek(0, SeekOrigin.Begin);
        
        using var memory = new MemoryStream();
        await fileStream.CopyToAsync(memory);

        var result = await _storage.SaveAsync(memory.ToArray(), file.ContentType);
        if (result.IsSuccess)
        {
            var creationResult = await _service.CreateAsync(new ImageCreationModel
            {
                Url = result.Url!,
                ContentKey = ContentKeyGenerator.Generate(nameof(ImageEntry), result.Url!),
                Owner = await _session.GetUserAsync()
            });
            
            if (!creationResult.IsSuccess)
                return Exception(creationResult.Exception!);

            var image = _mapper.Map<ImageResponse>(creationResult.Result);
            return Ok(image);
        }
        
        return Exception(result.Exception!);
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandom(int count = 1)
    {
        if (count > 20)
        {
            var limitException = new BadRequestException("Too many selection count");
            return Exception(limitException);
        }
        
        var random = await _service.GetRandomAsync(count);
        return Ok(_mapper.Map<ImageResponse[]>(random));
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