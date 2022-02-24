using HarmonyLib;

namespace BackToThePast.HideNoFail
{
    [HarmonyPatch(typeof(scnCLS), "ToggleNoFail")]
    public static class ToggleNoFailPatch1
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideNoFail;
        }
    }

    [HarmonyPatch(typeof(scnEditor), "ToggleNoFail")]
    public static class ToggleNoFailPatch2
    {
        public static bool Prefix()
        {
            return !Main.Settings.hideNoFail;
        }
    }
}
