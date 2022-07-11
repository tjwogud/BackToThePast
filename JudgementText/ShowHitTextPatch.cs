using BackToThePast.Utils;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BackToThePast.JudgementText
{
    [HarmonyPatch(typeof(scrController), "ShowHitText")]
    public static class ShowHitTextPatch
    {
        public static bool Prefix(scrController __instance, HitMargin hitMargin, ref Vector3 position, float angle)
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
                        scrFloor floor = scrLevelMaker.instance.listFloors[__instance.chosenplanet.other.currfloor.seqID - 1];
                        position = floor.transform.position;
                        position.y++;
                        break;
                }
            if (Main.Settings.forceJudgeCount)
            {
                for (int i = 0; i < Main.Settings.judgeCount; i++)
                {
                    scrHitTextMesh scrHitTextMesh = scrController.instance.Get<Dictionary<HitMargin, scrHitTextMesh[]>>("cachedHitTexts")[hitMargin][i];
                    if (scrHitTextMesh.dead)
                    {
                        scrHitTextMesh.Show(position, angle);
                        return false;
                    }
                }
                return false;
            }
            return true;
        }
    }
}
