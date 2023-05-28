using BackToThePast.Patch;
using HarmonyLib;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrPortal), "get_jumpPosition")]
    public static class jumpPositionPatch
    {
        public static bool Prepare()
        {
            return AccessTools.Property(typeof(scrPortal), "jumpPosition") != null;
        }

        public static bool Prefix(scrPortal __instance, ref Vector2Int __result)
        {
            if (__instance.world != "BackToThePast.OldXO")
                return true;
            __result = (Vector2Int)Main.xoLevelMeta["floorPos"];
            return false;
        }
    }
}
