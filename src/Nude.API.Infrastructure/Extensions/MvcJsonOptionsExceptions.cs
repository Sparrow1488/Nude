using Microsoft.AspNetCore.Mvc;
using Nude.API.Infrastructure.Configurations.Json;

namespace Nude.API.Infrastructure.Extensions;

public static class MvcJsonOptionsExceptions
{
    public static void BindOptions(this MvcNewtonsoftJsonOptions options)
    {
        var settings = JsonSettingsProvider.CreateDefault();
        
        options.SerializerSettings.Culture = settings.Culture;
        options.SerializerSettings.Formatting = settings.Formatting;
        options.SerializerSettings.Converters = settings.Converters;
        options.SerializerSettings.ContractResolver = settings.ContractResolver;
    }
}