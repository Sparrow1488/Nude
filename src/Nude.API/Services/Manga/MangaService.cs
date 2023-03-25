using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Exceptions;
using Nude.Parsers.NudeMoon;

namespace Nude.API.Services.Manga;

public class MangaService : IMangaService
{
    private readonly AppDbContext _context;
    private readonly INudeParser _nudeParser;
    private readonly IMapper _mapper;

    public MangaService(
        AppDbContext context, 
        INudeParser nudeParser,
        IMapper mapper)
    {
        _context = context;
        _nudeParser = nudeParser;
        _mapper = mapper;
    }

    public async Task<MangaResponse> GetByIdAsync(int id)
    {
        var manga = await FindMangaAsync(x => x.Id == id)
            ?? throw new NotFoundException("Manga not found", id.ToString(), "Manga");

        return _mapper.Map<MangaResponse>(manga);
    }

    public Task<MangaResponse> GetByUrlAsync(string url)
    {
        var mangaId = _nudeParser.Helper.GetIdFromUrl(url);
        return GetByExternalIdAsync(mangaId);
    }

    public async Task<MangaResponse> GetByExternalIdAsync(string externalId)
    {
        var manga = await FindMangaAsync(x => x.ExternalId == externalId)
            ?? throw new NotFoundException("Manga not found", externalId, "Manga");

        return _mapper.Map<MangaResponse>(manga);
    }

    private Task<Models.Mangas.Manga?> FindMangaAsync(Expression<Func<Models.Mangas.Manga, bool>> func)
    {
        return _context.Mangas
            .Include(x => x.Images)
            .ThenInclude(x => x.Url)
            .Include(x => x.Author)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(func);
    }
}