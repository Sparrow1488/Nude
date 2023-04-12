using Nude.API.Models.Formats;

namespace Nude.API.Models.Abstractions;

public interface IFormatContentEntry : IContentEntry
{
    ICollection<Format> Formats { get; set; }
}