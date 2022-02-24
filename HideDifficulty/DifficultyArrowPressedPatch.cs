using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    [HarmonyPatch(typeof(scrUIController), "DifficultyArrowPressed")]
    public static class DifficultyArrowPressedPatch
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideDifficulty;
        }
    }
}
