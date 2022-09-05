using BackToThePast.Patch;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch(typeof(scnEditor), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                HideDifficultyTweak.ToggleDifficulty(false);
        }
    }
}
