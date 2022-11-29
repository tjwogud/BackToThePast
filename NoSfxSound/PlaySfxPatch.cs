using BackToThePast.Patch;
using System.Linq;
using System.Reflection;

namespace BackToThePast.NoSfxSound
{
    [BTTPPatch]
    public static class PlaySfxPatch
    {
        public static MethodBase TargetMethod()
        {
            MethodBase playSfx = typeof(scrSfx).GetMethods().First(m => m.Name == "PlaySfx" && m.GetParameters().Length != 0 && m.GetParameters()[0].ParameterType == typeof(SfxSound));
            return playSfx;
        }

        public static bool Prefix(SfxSound __0)
        {
            switch (__0)
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
