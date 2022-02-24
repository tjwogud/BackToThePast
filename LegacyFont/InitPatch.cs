using BackToThePast.Utils;
using HarmonyLib;

namespace BackToThePast.LegacyFont
{
    [HarmonyPatch(typeof(scrHitTextMesh), "Init")]
    public static class InitPatch
    {
        private static FontData prev;

        public static void Prefix()
        {
            prev = RDString.fontData;
            if (Main.Settings.legacyFont && Main.Settings.butNotJudgement)
                typeof(RDString).Set("fontData", Main.font);
        }

        public static void Postfix()
        {
            typeof(RDString).Set("fontData", prev);
        }
    }
}
