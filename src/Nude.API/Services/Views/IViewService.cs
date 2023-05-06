using Nude.API.Models.Collections;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas;
using Nude.API.Models.Users;
using Nude.API.Models.Views;

namespace Nude.API.Services.Views;

public interface IViewService
{
    Task<View> CreateViewAsync(User user, MangaEntry manga);
    Task<View> CreateViewAsync(User user, ImageEntry image);
    Task<View> CreateViewAsync(User user, ImageCollection imageCollection);
}