using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Models.Formats;

namespace Nude.API.Infrastructure.Converters;

public class FormattedContentResponseConverter : JsonConverter<FormattedContentResponse>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer, 
        FormattedContentResponse? value, 
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override FormattedContentResponse? ReadJson(
        JsonReader reader, 
        Type objectType, 
        FormattedContentResponse? value, 
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
            FormatType.Telegraph => jObject.ToObject<TelegraphContentResponse>(),
            _ => throw new NoJsonConverterException(
                $"Not all methods are configured in {nameof(FormattedContentResponseConverter)}")
        };
    }
}