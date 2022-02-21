using HarmonyLib;

namespace BackToThePast.LegacyFont
{
    [HarmonyPatch(typeof(RDString), "GetFontDataForLanguage")]
    public static class GetFontDataForLanguagePatch
    {
        public static void Postfix(ref FontData __result)
        {
            if (!Main.Settings.legacyFont)
                return;
            __result.font = Main.legacyFont;
            __result.fontScale = 0.75f;
            __result.lineSpacing = 1;
        }
    }
}
