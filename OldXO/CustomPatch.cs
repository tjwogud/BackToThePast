using BackToThePast.Patch;
using BackToThePast.Utils;
using System;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrMistakesManager), "SaveCustom")]
    public static class SaveCustomPatch
    {
        public static bool Prefix(scrMistakesManager __instance, bool wonLevel, float multiplier, ref object __result)
        {
            if (ADOBase.customLevel == null
                || ADOBase.customLevel.levelPath != "BackToThePast.OldXO")
                return true;
            __result = __instance.Method("Save", new object[] { 1972, wonLevel, multiplier}, new Type[] { typeof(int), typeof(bool), typeof(float) });
            return false;
        }
    }

    [BTTPPatch(typeof(Persistence), "GetCustomWorldAttempts")]
    public static class GetCustomWorldAttemptsPatch
    {
        public static bool Prefix(ref int __result)
        {
            if (ADOBase.customLevel == null
                || ADOBase.customLevel.levelPath != "BackToThePast.OldXO")
                return true;
            __result = Persistence.GetWorldAttempts(1972);
            return false;
        }
    }

    [BTTPPatch(typeof(Persistence), "SetCustomWorldAttempts")]
    public static class SetCustomWorldAttemptsPatch
    {
        public static bool Prefix(int attempts)
        {
            if (ADOBase.customLevel == null
                || ADOBase.customLevel.levelPath != "BackToThePast.OldXO")
                return true;
            Persistence.SetWorldAttempts(1972, attempts);
            return false;
        }
    }
}
