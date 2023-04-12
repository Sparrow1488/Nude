using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Infrastructure.Exceptions.Internal;
using Nude.API.Models.Notifications.Details;

namespace Nude.API.Infrastructure.Converters;

public class NotificationDetailsConverter : JsonConverter<NotificationDetails>
{
    public override bool CanWrite => false;

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
            nameof(FormattingProgressDetails) => jObject.ToObject<FormattingProgressDetails>(),
            nameof(ContentTicketChangedDetails) => jObject.ToObject<ContentTicketChangedDetails>(),
            nameof(FormattingStatusDetails) => jObject.ToObject<FormattingStatusDetails>(),
            _ => throw new JsonConverterForgotException(nameof(NotificationDetailsConverter))
        };
    }
    
    public override void WriteJson(JsonWriter writer, NotificationDetails? value, JsonSerializer serializer)
        => throw new NotImplementedException();
}