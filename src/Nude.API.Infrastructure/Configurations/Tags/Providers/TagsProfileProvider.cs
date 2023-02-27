using Microsoft.Extensions.DependencyInjection;
using Nude.API.Infrastructure.Configurations.Tags.Profiles;

namespace Nude.API.Infrastructure.Configurations.Tags.Providers;

public abstract class TagsProfileProvider
{
    private IServiceProvider? _provider;
    
    public TProfile? Get<TProfile>() where TProfile : TagsProfile
    {
        if (_provider == null)
        {
            _provider = ConfigureProfiles(new ServiceCollection());
        }

        return _provider.GetService<TProfile>();
    }

    protected abstract IServiceProvider ConfigureProfiles(IServiceCollection services);
}