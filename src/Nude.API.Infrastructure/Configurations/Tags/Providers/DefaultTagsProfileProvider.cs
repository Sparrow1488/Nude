using Microsoft.Extensions.DependencyInjection;
using Nude.API.Infrastructure.Configurations.Tags.Profiles;

namespace Nude.API.Infrastructure.Configurations.Tags.Providers;

public class DefaultTagsProfileProvider : TagsProfileProvider
{
    protected override IServiceProvider ConfigureProfiles(IServiceCollection services)
    {
        var tagsMap = new Dictionary<string, string>
        {
            {"тэг1", "Tag_1"}
        };
        services.AddSingleton(_ => new DefaultTagsProfile(tagsMap));
        
        throw new NotImplementedException("Hueta");
    }
}