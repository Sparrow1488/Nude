using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Models.Formats;
using Nude.API.Services.Mangas;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Controllers;

[ApiController, Route("manga")]
public class MangaController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IMangaService _service;

    public MangaController(
        IMapper mapper,
        AppDbContext context,
        IMangaService service)
    {
        _mapper = mapper;
        _context = context;
        _service = service;
    }

    [HttpPost]
    public async Task Create()
    {
        var images = new List<string>
        {
            "img1.png",
            "img2.png",
            "img3.png",
        };
        
        var tags = new List<string> { "asd", "dfg", "gfj" };

        await _service.CreateAsync(
            "Test",
            "Lorem ipsum",
            images,
            tags,
            author: "No name",
            externalSourceUrl: "https://hueta.com/1488",
            externalSourceId: "1488");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _service.GetByIdAsync(id);

        if (manga != null)
        {
            return Ok(_mapper.Map<NewMangaResponse>(manga));
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> FindBySourceUrl(string sourceUrl, FormatType? format)
    {
        var manga = await _service.FindBySourceUrlAsync(sourceUrl, format);

        if (manga != null)
        {
            return Ok(_mapper.Map<NewMangaResponse>(manga));
        }

        return NotFound();
    }
}