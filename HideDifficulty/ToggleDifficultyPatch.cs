using BackToThePast.Patch;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch(typeof(EditorDifficultySelector), "ToggleDifficulty")]
    public static class ToggleDifficultyPatch
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideDifficulty;
        }
    }
}
