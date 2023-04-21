namespace Nude.API.Infrastructure.Services.Randomizers;

public interface IRandomizer
{
    void Shuffle(IList<int> list);
}