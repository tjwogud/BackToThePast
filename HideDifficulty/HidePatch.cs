using BackToThePast.Patch;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace BackToThePast.HideDifficulty
{
    [BTTPPatch]
    public static class HidePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(scnEditor), "Start");
            yield return AccessTools.Method(typeof(scnEditor), "SwitchToEditMode");
            yield return AccessTools.Method(typeof(scrUIController), "ShowDifficultyContainer");
            yield return AccessTools.Method(typeof(EditorDifficultySelector), "SetChangeable");
        }

        public static void Postfix()
        {
            if (Main.Settings.hideDifficulty)
                HideDifficultyTweak.ToggleDifficulty(false);
        }
    }
}
