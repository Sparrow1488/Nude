using Nude.API.Models.Abstractions;
using Nude.API.Models.Messages;

namespace Nude.API.Models.Users;

public class TelegramUser : IEntity
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public ICollection<UserMessage> Messages { get; set; } = null!;
}