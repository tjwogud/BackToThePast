using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackToThePast.NoSfxSound
{
    [HarmonyPatch]
    public static class PlaySfxPatch
    {
        public static MethodBase TargetMethod()
        {
            MethodBase playSfx = typeof(scrSfx).GetMethod("PlaySfx");
            MethodBase legacyPlaySfx = typeof(scrConductor).GetMethod("PlaySfx");
            return playSfx ?? legacyPlaySfx;
        }

        public static bool Prefix(SfxSound sound)
        {
            switch (sound)
            {
                case SfxSound.PurePerfect:
                    return !Main.Settings.disablePurePerfectSound;
                case SfxSound.ScreenWipeIn:
                case SfxSound.ScreenWipeOut:
                    return !Main.Settings.disableWindSound;
            }
            return true;
        }
    }
}
