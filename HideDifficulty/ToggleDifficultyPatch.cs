using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    [HarmonyPatch(typeof(EditorDifficultySelector), "ToggleDifficulty")]
    public static class ToggleDifficultyPatch
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideDifficulty;
        }
    }
}
