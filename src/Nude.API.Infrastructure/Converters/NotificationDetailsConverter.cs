using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Models.Notifications.Details;

namespace Nude.API.Infrastructure.Converters;

public class NotificationDetailsConverter : JsonConverter<NotificationDetails>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer, 
        NotificationDetails? value, 
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override NotificationDetails? ReadJson(
        JsonReader reader, 
        Type objectType, 
        NotificationDetails? existingValue,
        bool hasExistingValue, 
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }
        
        var jObject = JObject.Load(reader);
        var detailsType = jObject["Type"]?.Value<string>();

        return detailsType switch
        {
            nameof(FormatTicketProgressDetails) => jObject.ToObject<FormatTicketProgressDetails>(),
            nameof(ContentTicketStatusChangedDetails) => jObject.ToObject<ContentTicketStatusChangedDetails>(),
            nameof(FormatTicketStatusChangedDetails) => jObject.ToObject<FormatTicketStatusChangedDetails>(),
            _ => throw new NoJsonConverterException(
                $"Not all methods are configured in {nameof(NotificationDetailsConverter)}")
        };
    }
}