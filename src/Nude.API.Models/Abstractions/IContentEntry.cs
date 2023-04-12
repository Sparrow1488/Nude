using Nude.API.Models.Formats;

namespace Nude.API.Models.Abstractions;

public interface IContentEntry
{
    string ContentKey { get; set; }
    ICollection<Format> Formats { get; set; }
}