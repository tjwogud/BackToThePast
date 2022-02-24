using HarmonyLib;

namespace BackToThePast.HideNoFail
{
    [HarmonyPatch(typeof(scnEditor), "SwitchToEditMode")]
    public static class SwitchToEditModePatch
    {
        public static void Postfix(scnEditor __instance)
        {
            if (Main.Settings.hideNoFail)
                Main.HideNoFail();
        }
    }
}
