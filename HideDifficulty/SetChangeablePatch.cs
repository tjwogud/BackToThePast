using BackToThePast.Patch;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch(typeof(EditorDifficultySelector), "SetChangeable")]
    public static class SetChangeablePatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                HideDifficultyTweak.ToggleDifficulty(false);
        }
    }
}
