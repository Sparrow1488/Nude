using System.Reflection;
using AutoMapper;

namespace Nude.Mapping.Utils;

public static class MapperInitializer
{
    public static void AssertConfigurationIsValid(Assembly assembly)
    {
        var profiles = GetProfiles(assembly);
        var configuration = new MapperConfiguration(x =>
        {
            foreach (var profile in profiles)
                x.AddProfile(profile);
        });
        configuration.AssertConfigurationIsValid();
    }

    public static Type[] GetProfiles(Assembly assembly)
    {
        var profileType = typeof(Profile);
        return assembly.GetTypes().Where(x => x.IsAssignableTo(profileType)).ToArray();
    }
}