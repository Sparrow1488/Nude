using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Infrastructure.Converters;

namespace Nude.API.Infrastructure.Configurations.Json;

public static class JsonSettingsProvider
{
    public static JsonSerializerSettings CreateDefault()
    {
        return Create(new SnakeCaseNamingStrategy());
    }
    
    public static JsonSerializerSettings Create(NamingStrategy namingStrategy)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = namingStrategy
            }
        };
        
        settings.Converters.Add(new NotificationDetailsConverter());
        settings.Converters.Add(new FormattedContentResponseConverter());

        return settings;
    }
}