using BackToThePast.Utils;
using HarmonyLib;
using UnityEngine;

namespace BackToThePast
{
    [HarmonyPatch(typeof(scrFloor), "UpdateIconSprite")]
    public static class UpdateIconSpritePatch
    {
        public static void Postfix(scrFloor __instance)
        {
            for (int i = 0; i < __instance.transform.childCount; i++)
            {
                SpriteRenderer renderer = __instance.transform.GetChild(i)?.GetComponent<SpriteRenderer>();
                if (renderer != null && (renderer.name == "arrow_renderer" || renderer.name == "arrow_outline_renderer"))
                {
                    renderer.transform.parent = null;
                    Object.Destroy(renderer);
                }
            }
            if (!Main.Settings.legacyTwirl || (__instance.floorIcon != FloorIcon.Swirl && __instance.floorIcon != FloorIcon.SwirlCW))
                return;
            float num = (float)scrMisc.GetAngleMoved((float)__instance.entryangle, (float)__instance.exitangle, !__instance.isCCW);
            if (Mathf.Abs(num) <= 1E-06f && !__instance.midSpin)
                num = 6.2831855f;
            __instance.SetIconSprite(__instance.isCCW ? Images.swirl_ccw : Images.swirl_cw);
            __instance.SetIconFlipped(false);
            float num2 = 0f;
            if (__instance.floorRenderer is FloorSpriteRenderer)
            {
                if (__instance.lm.lm2 == null)
                    __instance.printe("lm2 is null for " + __instance.name);
                float num3 = __instance.lm.lm2.BigTiles ? -1.5707964f : 1.5707964f;
                num2 = (float)(((scrMisc.mod((float)(__instance.exitangle - __instance.entryangle), 6.2831854820251465) <= 3.1415927410125732) ? __instance.entryangle : __instance.exitangle) - num3);
            }
            float num4 = -(float)__instance.entryangle + 1.5707964f - num / 2f * (__instance.isCCW ? -1 : 1) - 1.5707964f + num2;
            __instance.SetIconAngle((__instance.floorRenderer is FloorSpriteRenderer) ? num4 : (-num4));
            __instance.SetIconOutlineSprite(__instance.isCCW ? Images.swirl_ccw_outline : Images.swirl_cw_outline);
            GameObject arrow_obj = new GameObject();
            arrow_obj.transform.parent = __instance.transform;
            SpriteRenderer arrow_renderer = arrow_obj.AddComponent<SpriteRenderer>();
            arrow_renderer.sortingOrder = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingOrder + 2;
            arrow_renderer.sortingLayerID = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerID;
            arrow_renderer.sortingLayerName = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerName;
            arrow_renderer.name = "arrow_renderer";
            arrow_renderer.sprite = __instance.isCCW ? Images.arrow_ccw : Images.arrow_cw;
            bool flag = num < 3.1415927f - Mathf.Pow(10f, -6f);
            arrow_renderer.color = flag ? Color.red : Color.blue;
            Vector3 localPosition = new Vector3(0.3f * Mathf.Cos(num4 + 90 * Mathf.Deg2Rad), 0.3f * Mathf.Sin(num4 + 90 * Mathf.Deg2Rad), 0f);
            arrow_renderer.transform.localPosition = localPosition;
            arrow_renderer.transform.localEulerAngles = new Vector3(0f, 0f, num4 * Mathf.Rad2Deg);
            if (__instance.controller.usingOutlines)
            {
                GameObject arrow_outline_obj = new GameObject();
                arrow_outline_obj.transform.parent = __instance.transform;
                SpriteRenderer arrow_outline_renderer = arrow_outline_obj.AddComponent<SpriteRenderer>();
                arrow_outline_renderer.sortingOrder = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingOrder + 1;
                arrow_outline_renderer.sortingLayerID = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerID;
                arrow_outline_renderer.sortingLayerName = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerName;
                arrow_outline_renderer.name = "arrow_outline_renderer";
                arrow_outline_renderer.sprite = __instance.isCCW ? Images.arrow_ccw_outline : Images.arrow_cw_outline;
                arrow_outline_renderer.transform.localPosition = localPosition;
                arrow_outline_renderer.transform.localEulerAngles = new Vector3(0f, 0f, num4 * Mathf.Rad2Deg);
            }
        }
    }

    [HarmonyPatch(typeof(scnEditor), "SelectFloor")]
    public static class SelectFloorPatch
    {
        public static void Postfix(scrFloor floorToSelect)
        {
        }
    }
}
