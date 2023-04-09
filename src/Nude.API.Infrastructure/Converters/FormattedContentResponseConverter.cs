using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Models.Formats;

namespace Nude.API.Infrastructure.Converters;

public class FormattedContentResponseConverter : JsonConverter<FormattedContentResponse>
{
    public override void WriteJson(
        JsonWriter writer, 
        FormattedContentResponse? value, 
        JsonSerializer serializer)
    {
        
    }

    public override FormattedContentResponse? ReadJson(
        JsonReader reader, 
        Type objectType, 
        FormattedContentResponse? value, 
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            var jObject = JObject.Load(reader);

            var formatTypeValue = jObject["type"]?.Value<long>();
            // var formatTypeValue = jObject["Type"]?.Value<long>();
            var formatType = (FormatType) formatTypeValue;
            if (formatType != null && formatType == FormatType.Telegraph)
            {
                return jObject.ToObject<TelegraphContentResponse>();
                // return jObject.Value<TelegraphContentResponse>();
            }
        }
        catch
        {
        }

        return null;
    }
}