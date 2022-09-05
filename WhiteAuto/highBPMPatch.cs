using BackToThePast.Patch;
using HarmonyLib;

namespace BackToThePast.WhiteAuto
{
    [BTTPPatch(typeof(scnEditor), "highBPM", MethodType.Getter)]
    public static class highBPMPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (Main.Settings.whiteAuto)
                __result = false;
        }
    }
}
