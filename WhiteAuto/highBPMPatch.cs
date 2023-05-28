using BackToThePast.Patch;
using HarmonyLib;

namespace BackToThePast.WhiteAuto
{
    [BTTPPatch(typeof(scnEditor), "get_highBPM")]
    public static class highBPMPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (Main.Settings.whiteAuto)
                __result = false;
        }
    }
}
