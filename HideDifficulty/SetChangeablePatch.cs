using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    [HarmonyPatch(typeof(EditorDifficultySelector), "SetChangeable")]
    public static class SetChangeablePatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                Main.HideDifficulty();
        }
    }
}
