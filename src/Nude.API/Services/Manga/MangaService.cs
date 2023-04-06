using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Services.Resolvers;

namespace Nude.API.Services.Manga;

public class MangaService : IMangaService
{
    private readonly AppDbContext _context;
    private readonly IMangaParserResolver _parserResolver;
    private readonly IMapper _mapper;

    public MangaService(
        AppDbContext context, 
        IMangaParserResolver parserResolver,
        IMapper mapper)
    {
        _context = context;
        _parserResolver = parserResolver;
        _mapper = mapper;
    }

    public async Task<MangaResponse> GetByIdAsync(int id)
    {
        var manga = await FindMangaAsync(x => x.Id == id)
            ?? throw new NotFoundException("Manga not found", id.ToString(), "Manga");

        return _mapper.Map<MangaResponse>(manga);
    }

    public async Task<MangaResponse> FindBySourceUrlAsync(string url)
    {
        var parser = await _parserResolver.ResolveByUrlAsync(url);
        var mangaId = parser.Helper.GetIdFromUrl(url);
        return await FindBySourceIdAsync(mangaId);
    }

    public async Task<MangaResponse> FindBySourceIdAsync(string id)
    {
        var manga = await FindMangaAsync(x => x.ExternalId == id)
            ?? throw new NotFoundException("Manga not found", id, "Manga");

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