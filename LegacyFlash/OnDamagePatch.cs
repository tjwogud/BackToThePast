using HarmonyLib;

namespace BackToThePast.LegacyFlash
{
    [HarmonyPatch(typeof(scrController), "OnDamage")]
    public static class OnDamagePatch
    {
        public static void Postfix()
        {
            if (Main.Settings.legacyFlash)
                scrFlash.OnDamage();
        }
    }
}
