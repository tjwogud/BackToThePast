using HarmonyLib;
using System.Reflection;

namespace BackToThePast.NoPracticeMode
{
    [HarmonyPatch(typeof(scrController), "PressedPracticeKey")]
    public static class PressedPracticeKeyPatch
    {
        public static bool Prepare(MethodBase original)
        {
            return original != null;
        }

        public static void Postfix(ref bool __result)
        {
            if (Main.Settings.noPracticeMode)
                __result = false;
        }
    }
}
