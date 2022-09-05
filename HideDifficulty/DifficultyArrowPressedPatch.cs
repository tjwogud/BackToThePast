using BackToThePast.Patch;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch(typeof(scrUIController), "DifficultyArrowPressed")]
    public static class DifficultyArrowPressedPatch
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideDifficulty;
        }
    }
}
