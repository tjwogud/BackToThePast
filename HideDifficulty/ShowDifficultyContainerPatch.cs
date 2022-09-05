using BackToThePast.Patch;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch(typeof(scrUIController), "ShowDifficultyContainer")]
    public static class ShowDifficultyContainerPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                HideDifficultyTweak.ToggleDifficulty(false);
        }
    }
}
