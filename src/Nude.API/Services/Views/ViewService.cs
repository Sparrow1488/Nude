using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Collections;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas;
using Nude.API.Models.Users;
using Nude.API.Models.Views;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Views;

public class ViewService : IViewService
{
    private readonly AppDbContext _context;

    public ViewService(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<View> CreateViewAsync(User user, MangaEntry manga) =>
        CreateAsync(user, v => v.Manga = manga,
            v => v.Manga!.Id == manga.Id);

    public Task<View> CreateViewAsync(User user, ImageEntry image) =>
        CreateAsync(user, v => v.Image = image,
            v => v.Image!.Id == image.Id);

    public Task<View> CreateViewAsync(User user, ImageCollection imageCollection) =>
        CreateAsync(user, v => v.ImageCollection = imageCollection, 
            v => v.ImageCollection!.Id == imageCollection.Id);

    public Task<View[]> FindByAsync(Expression<Func<View, bool>> filter)
    {
        return _context.Views
            .IncludeDependencies()
            .Where(filter)
            .ToArrayAsync();
    }

    private async Task<View> CreateAsync(
        User user, 
        Action<View> config,
        Expression<Func<View, bool>> checkViewed)
    {
        var view = await _context.Views.FirstOrDefaultAsync(checkViewed);

        if (view is null)
        {
            view = CreateConfiguredView(user, config);
        
            await _context.AddAsync(view);
            await _context.SaveChangesAsync();
        }
        
        return view;
    }

    private static View CreateConfiguredView(User user, Action<View> configure)
    {
        var view = new View
        {
            User = user,
        };
        
        configure.Invoke(view);
        return view;
    }
}