using HarmonyLib;

namespace BackToThePast.HideNoFail
{
    [HarmonyPatch(typeof(scnEditor), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideNoFail)
                Main.HideNoFail();
        }
    }
}
