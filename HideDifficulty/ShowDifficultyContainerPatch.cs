using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    [HarmonyPatch(typeof(scrUIController), "ShowDifficultyContainer")]
    public static class ShowDifficultyContainerPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                Main.HideDifficulty();
        }
    }
}
