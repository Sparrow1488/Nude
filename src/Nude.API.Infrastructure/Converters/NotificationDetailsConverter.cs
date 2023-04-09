using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Models.Notifications.Details;

namespace Nude.API.Infrastructure.Converters;

public class NotificationDetailsConverter : JsonConverter<NotificationDetails>
{
    public override void WriteJson(
        JsonWriter writer, 
        NotificationDetails? value, 
        JsonSerializer serializer)
    {
        
    }

    public override NotificationDetails? ReadJson(JsonReader reader, Type objectType, NotificationDetails? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        try
        {
            var jObject = JObject.Load(reader);

            var detailsType = jObject["DetailsType"]?.Value<string>(); // FIX: с мелкой

            if (detailsType != null)
            {
                return detailsType switch
                {
                    nameof(FormatTicketProgressDetails) => jObject.ToObject<FormatTicketProgressDetails>(),
                    nameof(ContentTicketStatusChangedDetails) => jObject.ToObject<ContentTicketStatusChangedDetails>(),
                    nameof(FormatTicketStatusChangedDetails) => jObject.ToObject<FormatTicketStatusChangedDetails>(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        catch
        {
            
        }
        return null;
    }
}