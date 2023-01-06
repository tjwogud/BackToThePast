using BackToThePast.Patch;
using UnityEngine;

namespace BackToThePast.ShowSpeedChange
{
	[BTTPPatch(typeof(scrFloor), "UpdateIconSprite")]
	public static class UpdateIconSpritePatch
    {
        public static void Prefix(scrFloor __instance)
        {
			switch (__instance.floorIcon)
            {
				case FloorIcon.Rabbit:
				case FloorIcon.DoubleRabbit:
				case FloorIcon.Snail:
				case FloorIcon.DoubleSnail:
				case FloorIcon.AnimatedRabbit:
				case FloorIcon.AnimatedDoubleRabbit:
				case FloorIcon.AnimatedSnail:
				case FloorIcon.AnimatedDoubleSnail:
				case FloorIcon.SameSpeed:
					if (Main.Settings.showSmallSpeedChange && scrLevelMaker.instance != null)
					{
						float prevSpeed;
						if (__instance.seqID > 0)
							prevSpeed = scrLevelMaker.instance.listFloors[__instance.seqID - 1].speed;
						else
							prevSpeed = 1f;
						float speedDifference = (__instance.speed - prevSpeed) / prevSpeed;
						if (Main.Settings.showDetailSpeedChange && Mathf.Abs(speedDifference) <= Main.Settings.minBpmToShowSpeedChange)
							__instance.floorIcon = FloorIcon.SameSpeed;
						else if (!Main.Settings.showDetailSpeedChange && Mathf.Abs(speedDifference) == 0)
							__instance.floorIcon = FloorIcon.SameSpeed;
						else
							__instance.floorIcon = (speedDifference > 0f)
								? ((Mathf.Abs(speedDifference) < 1.05f) ? FloorIcon.Rabbit : FloorIcon.DoubleRabbit)
								: ((1f - Mathf.Abs(speedDifference) > 0.45f) ? FloorIcon.Snail : FloorIcon.DoubleSnail);
					}
					break;
            }
        }
    }
}
