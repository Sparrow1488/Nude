using RandN;

namespace Nude.API.Infrastructure.Services.Randomizers;

public class CryptoRandomizer : IRandomizer
{
    private readonly SmallRng _random;

    public CryptoRandomizer()
    {
        _random = SmallRng.Create();
    }
    
    public void Shuffle(IList<int> list) =>
        _random.ShuffleInPlace(list);
}