
using HarmonyLib;

namespace BackToThePast.NoResult
{
    [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
    public static class OnLandOnPortalPatch
    {
        public static void Postfix(scrController __instance)
        {
            if (Main.Settings.noResult)
            {
                __instance.txtCongrats.text = string.Empty;
                __instance.txtResults.gameObject.SetActive(false);
                __instance.txtAllStrictClear.text = string.Empty;
            }
        }
    }
}
