namespace Nude.Helpers;

public static class NudeEnvironment
{
    private const string NudeRootDirectoryName = "Nude";
    
    public static string GetNudeStoreCredentialsPath()
    {
        return Path.Combine(GetNudeAppDataPath(), "Store", "Credentials");
    }
    
    public static string GetNudeAppDataPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, NudeRootDirectoryName);
    }
}