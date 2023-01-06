using BackToThePast.Patch;

namespace BackToThePast.HideAnnounceSign
{
    [BTTPPatch(typeof(NewsSign), "Awake")]
    public static class AwakePatch
    {
        public static bool Prefix()
        {
            if (Main.Settings.disableAnnounceSign)
            {
                HideAnnounceSignTweak.ToggleSign(false);
                return false;
            }
            return true;
        }
    }
}
