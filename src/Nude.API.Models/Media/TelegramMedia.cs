using Nude.API.Models.Abstractions;
using Nude.API.Models.Enums;

namespace Nude.API.Models.Media;

public class TelegramMedia : IEntity, IHasContentKey
{
    public int Id { get; set; }
    public string ContentKey { get; set; } = null!;
    public string FileId { get; set; } = null!;
    public TelegramMediaType MediaType { get; set; }
}