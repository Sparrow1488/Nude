using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Manga.Responses;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Exceptions;

namespace Nude.API.Services.Manga;

public class MangaService : IMangaService
{
    private readonly FixedAppDbContext _context;
    private readonly IMapper _mapper;

    public MangaService(
        FixedAppDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MangaResponse> GetByIdAsync(int id)
    {
        var manga = await FindMangaAsync(x => x.Id == id)
            ?? throw new NotFoundException("Manga not found", id.ToString(), "Manga");

        return _mapper.Map<MangaResponse>(manga);
    }

    public Task<MangaResponse> FindBySourceUrlAsync(string url)
    {
        // var parser = await _parserResolver.ResolveByUrlAsync(url);
        // var mangaId = parser.Helper.GetIdFromUrl(url);
        // return await FindBySourceIdAsync(mangaId);
        throw new NotImplementedException();
    }

    public async Task<MangaResponse> FindBySourceIdAsync(string id)
    {
        var manga = await FindMangaAsync(x => x.ExternalMeta != null && x.ExternalMeta.SourceId == id)
            ?? throw new NotFoundException("Manga not found", id, "Manga");

        return _mapper.Map<MangaResponse>(manga);
    }

    private Task<Models.Mangas.MangaEntry?> FindMangaAsync(Expression<Func<Models.Mangas.MangaEntry, bool>> func)
    {
        return _context.Mangas
            .Include(x => x.Images)
            .ThenInclude(x => x.Url)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(func);
    }
}