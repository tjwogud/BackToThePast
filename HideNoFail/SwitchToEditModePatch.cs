using BackToThePast.Patch;

namespace BackToThePast.HideNoFail
{
    [BTTPPatch(typeof(scnEditor), "SwitchToEditMode")]
    public static class SwitchToEditModePatch
    {
        public static void Postfix(scnEditor __instance)
        {
            if (Main.Settings.hideNoFail)
                HideNoFailTweak.ToggleNoFail(false);
        }
    }
}
