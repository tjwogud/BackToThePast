using HarmonyLib;
using UnityEngine;

namespace BackToThePast.OldPracticeMode
{
    [HarmonyPatch(typeof(scrController), "Fail2_Update")]
    public static class Fail2_UpdatePatch
    {
        public static bool Prefix()
        {
            if (Main.Settings.oldPracticeMode && scrController.instance.practiceAvailable && !GCS.practiceMode && Input.GetKeyDown(KeyCode.P))
            {
                scrController.instance.SetPracticeMode(true);
                return false;
            }
            return true;
        }
    }
}
