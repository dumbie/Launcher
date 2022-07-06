namespace Launcher
{
    public class Strings
    {
        public static string ReplaceLauncherName(string replaceName)
        {
            return replaceName.Replace("Launcher-", string.Empty).Replace("Launcher - ", string.Empty).Replace("-Launcher", string.Empty).Replace(" - Launcher", string.Empty);
        }
    }
}