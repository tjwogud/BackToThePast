using BackToThePast.Patch;
using BackToThePast.Utils;
using UnityEngine.UI;

namespace BackToThePast.LegacyFont
{
    [BTTPPatch(typeof(RDString), "SetLocalizedFont", typeof(Text))]
    public static class SetLocalizedFontPatch
    {
        private static FontData prev;

        public static void Prefix(Text text)
        {
            prev = RDString.fontData;
            if (Main.Settings.legacyFont && ((Main.Settings.butNotTitle && text.name == "txtLevelName") || (Main.Settings.butNotSetting && text.transform.root.name == "PauseMenu(Clone)")))
                typeof(RDString).Set("fontData", Main.originFont);
        }

        public static void Postfix()
        {
            typeof(RDString).Set("fontData", prev);
        }
    }
}
