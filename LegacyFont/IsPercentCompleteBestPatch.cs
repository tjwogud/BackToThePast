using HarmonyLib;

namespace BackToThePast.LegacyFont
{
    [HarmonyPatch(typeof(scrController), "IsPercentCompleteBest")]
    public static class IsPercentCompleteBestPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (scnEditor.instance != null)
                __result = false;
        }
    }
}
