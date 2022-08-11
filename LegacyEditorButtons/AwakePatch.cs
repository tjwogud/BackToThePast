using HarmonyLib;

namespace BackToThePast.LegacyEditorButtons
{
    [HarmonyPatch(typeof(scnEditor), "Awake")]
    public static class AwakePatch
    {
        public static void Postfix()
        {
            Main.ChangeEditorButtons(Main.Settings.legacyEditorButtonsPositions);
            Main.RemoveShadowAddOutline(Main.Settings.legacyEditorButtonsDesigns);
        }
    }
}
