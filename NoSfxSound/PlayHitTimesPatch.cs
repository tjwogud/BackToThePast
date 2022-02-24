using HarmonyLib;

namespace BackToThePast.NoSfxSound
{
    [HarmonyPatch(typeof(scrConductor), "PlayHitTimes")]
    public static class PlayHitTimesPatch
    {
        private static bool prev1;
        private static bool prev2;

        public static void Prefix(scrConductor __instance)
        {
            prev1 = __instance.fastTakeoff;
            if (Main.Settings.disableCountdownSound)
                __instance.fastTakeoff = true;
            prev2 = __instance.playEndingCymbal;
            if (Main.Settings.disableEndingSound)
                __instance.playEndingCymbal = false;
        }

        public static void Postfix(scrConductor __instance)
        {
            __instance.fastTakeoff = prev1;
            __instance.playEndingCymbal = prev2;
        }
    }
}
