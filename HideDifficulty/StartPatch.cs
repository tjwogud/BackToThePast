using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    [HarmonyPatch(typeof(scnEditor), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                Main.HideDifficulty();
        }
    }
}
