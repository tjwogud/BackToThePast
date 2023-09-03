using BackToThePast.Patch;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace BackToThePast.HideNoFail
{
    [BTTPPatch]
    public static class HidePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(scnEditor), "SwitchToEditMode");
            yield return AccessTools.Method(typeof(scnEditor), "Start");
        }
        
        public static void Postfix()
        {
            if (Main.Settings.hideNoFail)
                HideNoFailTweak.ToggleNoFail(false);
        }
    }
}
