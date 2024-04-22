using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
namespace _7tv_automation_tests.Helpers
{
    public static class DataForTests
    {
        public static readonly string longString = new string('a', 600);
        public const string alphabet = "abcdefghijklmnopqrstuwxyz";

        public static readonly ProfileData testingProfile = new ProfileData {profileId = "60ae3c29b2ecb015051f8f9a", emoteSetId ="63ae1ec891cc626e2620e3ca", profileName = "NymN"};


    //private static string GetOriginPath()
    //{
    //    string location = Assembly.GetEntryAssembly().Location;
    //    string executableDirectory = Path.GetDirectoryName(location);

    //    return executableDirectory;
    //}

    //public static string ExecutableDirectory => GetOriginPath();
    //public static string ScreenshotResultPath => Path.Combine(ExecutableDirectory, "ScreenshotResults");
}
}
