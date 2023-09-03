using BackToThePast.Patch;

namespace BackToThePast.HideAnnounceSign
{
    [BTTPPatch(typeof(NewsSign), "Awake")]
    public static class AwakePatch
    {
        public static void Postfix()
        {
            if (Main.Settings.disableAnnounceSign)
            {
                HideAnnounceSignTweak.ToggleSign(false);
            }
        }
    }
}
