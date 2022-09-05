using BackToThePast.Patch;
using BackToThePast.Utils;
using UnityEngine;
using static BackToThePast.Patch.BTTPPatchAttribute;

namespace BackToThePast.LegacyCLS
{
    [BTTPPatch(typeof(scnCLS), "Awake")]
    [IgnoreSetting]
    public static class AwakePatch
    {
        public static bool Prepare()
        {
            return Main.optionsPanelsCLS != null;
        }

        public static void Postfix()
        {
            scnCLS.instance.gameObject.GetOrAddComponent<WorkshopShortcut>();
            Object.Destroy(scnCLS.instance.transform.Find("LevelInfoCanvas").Find("HelpContainer").Find("HelpOrder").GetComponent<scrTextChanger>());
            scnCLS.instance.Get("optionsPanels").Method("UpdateOrderText");
            LegacyCLSTweak.Toggle(Main.Settings.legacyCLS);
        }
    }
}
