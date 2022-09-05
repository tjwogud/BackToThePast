using BackToThePast.Patch;

namespace BackToThePast.LegacyEditorButtons
{
    [BTTPPatch(typeof(scnEditor), "Awake")]
    public static class AwakePatch
    {
        public static void Postfix()
        {
            LegacyEditorButtonsTweak.ChangeEditorButtons(Main.Settings.legacyEditorButtonsPositions);
            LegacyEditorButtonsTweak.RemoveShadowAddOutline(Main.Settings.legacyEditorButtonsDesigns);
        }
    }
}
