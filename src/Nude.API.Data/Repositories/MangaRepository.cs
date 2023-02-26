using Nude.API.Data.Contexts;
using Nude.Models.Mangas;

namespace Nude.API.Data.Repositories;

public class MangaRepository : IRepository<Manga>
{
    private readonly AppDbContext _context;

    public MangaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Manga> AddAsync(Manga manga)
    {
        await _context.Mangas.AddAsync(manga);
        return manga;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}