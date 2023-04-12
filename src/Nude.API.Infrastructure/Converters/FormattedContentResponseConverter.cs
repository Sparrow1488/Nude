using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Infrastructure.Exceptions.Internal;
using Nude.API.Models.Formats;

namespace Nude.API.Infrastructure.Converters;

public class FormattedContentResponseConverter : JsonConverter<FormatResponse>
{
    public override bool CanWrite => false;

    public override FormatResponse? ReadJson(
        JsonReader reader, 
        Type objectType, 
        FormatResponse? value, 
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }
        
        var jObject = JObject.Load(reader);
        var formatTypeValue = jObject["Type"]?.Value<long>();

        if (formatTypeValue == null) return null;

        var formatType = (FormatType) formatTypeValue;
        return formatType switch
        {
            FormatType.Telegraph => jObject.ToObject<TelegraphFormatResponse>(),
            _ => throw new JsonConverterForgotException(nameof(FormattedContentResponseConverter))
        };
    }
    
    
    public override void WriteJson(JsonWriter writer, FormatResponse? value, JsonSerializer serializer)
        => throw new NotImplementedException();
}