namespace Nude.API.Infrastructure.Configurations.Tags.Profiles;

public abstract class TagsProfile
{
    private readonly Dictionary<string, string> _maps;
    
    public TagsProfile(Dictionary<string, string> maps)
    {
        _maps = maps;
    }
    
    protected virtual string UnmappedDefaultTagValue => "no_map";

    public string this[string tagKey] 
    {
        get
        {
            _maps.TryGetValue(tagKey, out var result);
            return result ?? UnmappedDefaultTagValue;
        }
    }
}