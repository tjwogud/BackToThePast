using HarmonyLib;
using UnityEngine;

namespace BackToThePast.ShowSmallSpeedChange
{
	[HarmonyPatch(typeof(scrFloor), "UpdateIconSprite")]
	public static class UpdateIconSpritePatch
    {
        public static void Prefix(scrFloor __instance)
        {
            if (Main.Settings.showSmallSpeedChange && __instance.floorIcon == FloorIcon.SameSpeed)
            {
				float num11;
				if (__instance.seqID > 0)
					num11 = __instance.lm.listFloors[__instance.seqID - 1].speed;
				else
					num11 = 1f;
				float num12 = (__instance.speed - num11) / num11;
				float num13 = Mathf.Abs(num12);
				__instance.floorIcon = (num12 > 0f) ? ((num13 < 1.05f) ? FloorIcon.Rabbit : FloorIcon.DoubleRabbit) : ((1f - num13 > 0.45f) ? FloorIcon.Snail : FloorIcon.DoubleSnail);
			}
        }
    }
}
