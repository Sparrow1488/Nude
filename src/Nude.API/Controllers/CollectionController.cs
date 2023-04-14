using AutoMapper;
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

[Route($"{ApiDefaults.CurrentVersion}/collections")]
public class CollectionController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IImagesService _imagesService;
    private readonly IImageCollectionsService _service;

    public CollectionController(
        IMapper mapper,
        IImagesService imagesService,
        IImageCollectionsService service)
    {
        _mapper = mapper;
        _imagesService = imagesService;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ImageCollectionCreateRequest request)
    {
        var images = await _imagesService.FindAsync(request.Images);

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

        var imagesIdSum = images.Sum(x => x.Id);
        var contentKeyRow = imagesIdSum + "+" + images.Count;
        var collection = await _service.CreateAsync(
            request.Title,
            request.Description,
            ContentKeyGenerator.Generate(nameof(ImageCollection), contentKeyRow),
            images
        );

        return Ok(_mapper.Map<ImageCollectionResponse>(collection));
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