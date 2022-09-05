using BackToThePast.Patch;

namespace BackToThePast.LegacyFlash
{
    [BTTPPatch(typeof(scrController), "OnDamage")]
    public static class OnDamagePatch
    {
        public static void Postfix()
        {
            if (Main.Settings.legacyFlash)
                scrFlash.OnDamage();
        }
    }
}
