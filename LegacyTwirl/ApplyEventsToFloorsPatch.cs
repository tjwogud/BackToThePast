using ADOFAI;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace BackToThePast.LegacyTwirl
{
    [HarmonyPatch(typeof(CustomLevel), "ApplyEventsToFloors", new Type[] { typeof(List<scrFloor>), typeof(LevelData), typeof(scrLevelMaker), typeof(List<LevelEvent>)})]
    public static class ApplyEventsToFloorsPatch
    {
        public static void Prefix()
        {
            foreach (TwirlRenderer renderer in UnityEngine.Object.FindObjectsOfType<TwirlRenderer>())
            {
                renderer.transform.parent = null;
                UnityEngine.Object.Destroy(renderer.gameObject);
            }
        }
    }
}
