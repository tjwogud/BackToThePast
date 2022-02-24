using HarmonyLib;
using UnityEngine;

namespace BackToThePast.LateJudgement
{
    [HarmonyPatch(typeof(scrController), "ShowHitText")]
    public static class ShowHitTextPatch
    {
        public static void Prefix(scrController __instance, HitMargin hitMargin, ref Vector3 position)
        {
            if (Main.Settings.lateJudgement)
                switch (hitMargin)
                {
                    case HitMargin.TooEarly:
                    case HitMargin.TooLate:
                    case HitMargin.FailMiss:
                    case HitMargin.FailOverload:
                        break;
                    default:
                        scrFloor floor = __instance.lm.listFloors[__instance.chosenplanet.other.currfloor.seqID - 1];
                        position = floor.transform.position;
                        position.y++;
                        break;
                }
        }
    }
}
