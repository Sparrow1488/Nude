using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Collections.Requests;
using Nude.API.Contracts.Collections.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Collections;
using Nude.API.Services.Collections;
using Nude.API.Services.Images;

namespace Nude.API.Controllers;

[Route("collections")]
public class CollectionController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly IImageCollectionService _service;

    public CollectionController(
        IMapper mapper,
        IImageService imageService,
        IImageCollectionService service)
    {
        _mapper = mapper;
        _service = service;
        _imageService = imageService;
    }

    [HttpPost, Authorize(Policies.Admin)]
    public async Task<IActionResult> Create(ImageCollectionCreateRequest request)
    {
        var images = await _imageService.FindAsync(request.Images);
        
        if (images.Count != request.Images.Length)
        {
            var foundContentKeys = images.Select(x => x.ContentKey);
            var notFoundImages = request.Images.Where(
                x => !foundContentKeys.Contains(x)
            ).ToList();
            
            return Exception(new NotFoundException(
                $"Some images not found by ContentKey ({notFoundImages.Count})")
            );
        }
        
        var contentKeyRow = images.Sum(x => x.Id) + "+" + images.Count;
        var contentKey = ContentKeyGenerator.Generate(nameof(ImageCollection), contentKeyRow);

        var result = await _service.CreateAsync(
            request.Title,
            request.Description,
            contentKey,
            images
        );

        if (result.IsSuccess)
        {
            return Ok(_mapper.Map<ImageCollectionResponse>(result.Result));
        }

        return Exception(result.Exception!);
    }

    [HttpGet]
    public async Task<IActionResult> FindBy(string contentKey)
    {
        var collection = await _service.FindByContentKeyAsync(contentKey);

        if (collection == null)
        {
            return Exception(new NotFoundException("Collection not found"));
        }

        return Ok(_mapper.Map<ImageCollectionResponse>(collection));
    }
}