using BackToThePast.Utils;
using HarmonyLib;

namespace BackToThePast.LegacyFont
{
    [HarmonyPatch(typeof(scrCountdown), "Awake")]
    public static class AwakePatch
    {
        private static FontData prev;

        public static void Prefix()
        {
            prev = RDString.fontData;
            if (Main.Settings.legacyFont && Main.Settings.butNotCountdown)
                typeof(RDString).Set("fontData", Main.font);
        }

        public static void Postfix()
        {
            typeof(RDString).Set("fontData", prev);
        }
    }
}
