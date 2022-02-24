
using HarmonyLib;

namespace BackToThePast.WhiteAuto
{
    [HarmonyPatch(typeof(scnEditor), "highBPM", MethodType.Getter)]
    public static class GetPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (Main.Settings.whiteAuto)
                __result = false;
        }
    }
}
