using BackToThePast.Patch;

namespace BackToThePast.HideNoFail
{
    [BTTPPatch(typeof(scnEditor), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            if (Main.Settings.hideNoFail)
                HideNoFailTweak.ToggleNoFail(false);
        }
    }
}
