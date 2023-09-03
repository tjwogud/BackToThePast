using BackToThePast.Patch;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch]
    public static class CancelChangePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(EditorDifficultySelector), "ToggleDifficulty");
            yield return AccessTools.Method(typeof(scrUIController), "DifficultyArrowPressed");
        }

        public static bool Prefix()
        {
            return !Main.Settings.hideDifficulty;
        }
    }
}
