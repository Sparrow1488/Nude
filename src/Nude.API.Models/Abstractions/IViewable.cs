using Nude.API.Models.Views;

namespace Nude.API.Models.Abstractions;

public interface IViewable
{
    ICollection<View> Views { get; set; }
}