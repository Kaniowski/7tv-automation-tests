﻿using _7tv_automation_tests.Helpers;

namespace _7tv_automation_tests.Misc
{
    public static class DataForTests
    {
        public static readonly string longString = new string('a', 600);
        public const string alphabet = "abcdefghijklmnopqrstuwxyz";

        public static readonly ProfileData testingProfile = new ProfileData { profileId = "60ae3c29b2ecb015051f8f9a", emoteSetId = "63ae1ec891cc626e2620e3ca", profileName = "NymN" };
    }
}
