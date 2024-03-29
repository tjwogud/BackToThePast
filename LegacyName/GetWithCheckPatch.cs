﻿using BackToThePast.Patch;

namespace BackToThePast.LegacyName
{
    [BTTPPatch(typeof(RDString), "GetWithCheck")]
    public static class GetWithCheckPatch
    {
        public static void Postfix(ref string __result)
        {
            if (Main.Settings.legacyTexts)
                switch (__result)
                {
                    case "눈폭풍":
                        __result = "눈폭충";
                        break;
                    case "세피아":
                        __result = "소피아";
                        break;
                    case "작곡가":
                        __result = "아티스트";
                        break;
                }
        }
    }
}
