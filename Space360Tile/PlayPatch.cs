using BackToThePast.Utils;
using HarmonyLib;
using UnityEngine;

namespace BackToThePast.Space360Tile
{
    [HarmonyPatch(typeof(scnEditor), "Play")]
    public static class PlayPatch
    {
        public static bool Prefix(scnEditor __instance)
        {
            if (Main.Settings.space360Tile && !Input.GetKeyDown(KeyCode.P) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space))
            {
                if (__instance.SelectionIsSingle())
                    __instance.Method("CreateFloorWithCharOrAngle", new object[] {
                        scrLevelMaker.instance.GetRotDirection(
                            scrLevelMaker.instance.GetRotDirection(
                                __instance.selectedFloors[0].floatDirection, true), true),
                        scrLevelMaker.instance.GetRotDirection(
                            scrLevelMaker.instance.GetRotDirection(
                                __instance.selectedFloors[0].stringDirection, true), true), true, true });
                return false;
            }
            return true;
        }
    }
}
