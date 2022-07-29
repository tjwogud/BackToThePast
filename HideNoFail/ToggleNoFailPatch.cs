using HarmonyLib;
using System.Reflection;

namespace BackToThePast.HideNoFail
{
    [HarmonyPatch]
    public static class ToggleNoFailPatch1
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(scnCLS), "ToggleNoFail") ?? AccessTools.Method(typeof(ADOBase).Assembly.GetType("OptionsPanelsCLS"), "ToggleNoFail");
        }

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
