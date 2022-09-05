using BackToThePast.Patch;
using UnityEngine;

namespace BackToThePast.LegacyFont
{
    [BTTPPatch(typeof(RDString), "GetFontDataForLanguage")]
    public static class GetFontDataForLanguagePatch
    {
        public static void Postfix(ref FontData __result, SystemLanguage language)
        {
            if (language == RDString.language)
                Main.font = __result;
            if (!Main.Settings.legacyFont)
                return;
            __result = Main.legacyFont;
        }
    }
}
