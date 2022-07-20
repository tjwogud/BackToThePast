using BackToThePast.Utils;
using HarmonyLib;

namespace BackToThePast.HideAlphaWarning
{
    [HarmonyPatch(typeof(scnSplash), "Start")]
    public static class StartPatch
    {
        public static bool Prefix(scnSplash __instance)
        {
            if (Main.Settings.disableAlphaWarning)
                __instance.Method("GoToMenu");
            return !Main.Settings.disableAlphaWarning;
        }
    }
}
