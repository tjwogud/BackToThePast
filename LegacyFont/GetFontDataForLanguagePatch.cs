﻿using BackToThePast.Patch;
using UnityEngine;

namespace BackToThePast.LegacyFont
{
    [BTTPPatch(typeof(RDString), "GetFontDataForLanguage")]
    public static class GetFontDataForLanguagePatch
    {
        public static void Postfix(ref FontData __result, SystemLanguage language)
        {
            if (language == RDString.language)
                Main.originFont = __result;
            if (Main.Settings.legacyFont)
                __result = Main.legacyFont;
            else if (Main.Settings.oldGodoMaum && __result.font.name == RDConstants.data.latinFont.name)
                __result = new FontData()
                {
                    font = Main.oldGodoMaum,
                    fontScale = __result.fontScale,
                    fontTMP = __result.fontTMP,
                    lineSpacing = __result.lineSpacing,
                    lineSpacingTMP = __result.lineSpacingTMP
                };
        }
    }
}
