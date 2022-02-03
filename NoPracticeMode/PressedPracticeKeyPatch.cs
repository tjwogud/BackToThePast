using HarmonyLib;

namespace BackToThePast.NoPracticeMode
{
    [HarmonyPatch(typeof(scrController), "PressedPracticeKey")]
    public static class PressedPracticeKeyPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (Main.Settings.noPracticeMode)
                __result = false;
        }
    }
}
