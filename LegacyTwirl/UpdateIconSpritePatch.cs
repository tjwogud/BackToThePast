using BackToThePast.Patch;
using UnityEngine;

namespace BackToThePast.LegacyTwirl
{
    [BTTPPatch(typeof(scrFloor), "UpdateIconSprite")]
    public static class UpdateIconSpritePatch
    {
        public static void Postfix(scrFloor __instance)
        {
            Object.DestroyImmediate(__instance.transform.Find("arrow_renderer")?.gameObject);
            Object.DestroyImmediate(__instance.transform.Find("arrow_outline_renderer")?.gameObject);
            if (!Main.Settings.legacyTwirl || __instance.isportal || (__instance.floorIcon != FloorIcon.Swirl && __instance.floorIcon != FloorIcon.SwirlCW))
                return;
            float num = (float)scrMisc.GetAngleMoved((float)__instance.entryangle, (float)__instance.exitangle, !__instance.isCCW);
            if (Mathf.Abs(num) <= 1E-06f && !__instance.midSpin)
                num = Mathf.PI * 2;
            __instance.SetIconSprite(__instance.isCCW ? Images.swirl_ccw : Images.swirl_cw);
            __instance.SetIconFlipped(false);
            float num2 = 0f;
            if (__instance.floorRenderer is FloorSpriteRenderer)
            {
                if (scrLevelMaker.instance.lm2 == null)
                    __instance.printe("lm2 is null for " + __instance.name);
                float num3 = scrLevelMaker.instance.lm2.BigTiles ? Mathf.PI / -2 : Mathf.PI / 2;
                num2 = (float)(((scrMisc.mod((float)(__instance.exitangle - __instance.entryangle), Mathf.PI * 2) <= Mathf.PI) ? __instance.entryangle : __instance.exitangle) - num3);
            }
            float num4 = -(float)__instance.entryangle + Mathf.PI / 2 - num / 2f * (__instance.isCCW ? -1 : 1) - Mathf.PI / 2 + num2;
            __instance.SetIconAngle((__instance.floorRenderer is FloorSpriteRenderer) ? num4 : (-num4));
            __instance.SetIconOutlineSprite(__instance.isCCW ? Images.swirl_ccw_outline : Images.swirl_cw_outline);
            if (Main.Settings.twirlWithoutArrow)
                return;
            GameObject arrow_obj = new GameObject();
            arrow_obj.transform.parent = __instance.transform;
            TwirlRenderer arrow = arrow_obj.AddComponent<TwirlRenderer>();
            arrow.outline = false;
            arrow.floor = __instance;
            arrow.renderer.sprite = __instance.isCCW ? Images.arrow_ccw : Images.arrow_cw;
            arrow.renderer.sortingLayerID = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerID;
            arrow.renderer.sortingLayerName = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerName;
            arrow.name = "arrow_renderer";
            Vector3 localPosition = new Vector3(0.3f * Mathf.Cos(num4 + 90 * Mathf.Deg2Rad), 0.3f * Mathf.Sin(num4 + 90 * Mathf.Deg2Rad), 0f);
            arrow.transform.localPosition = localPosition;
            arrow.transform.localEulerAngles = new Vector3(0f, 0f, num4 * Mathf.Rad2Deg);
            bool flag = num < Mathf.PI - Mathf.Pow(10f, -6f);
            arrow.renderer.color = flag ? Color.red : Color.blue;
            if (scrController.instance.usingOutlines)
            {
                GameObject arrow_outline_obj = new GameObject();
                arrow_outline_obj.transform.parent = __instance.transform;
                TwirlRenderer arrow_outline = arrow_outline_obj.AddComponent<TwirlRenderer>();
                arrow_outline.outline = true;
                arrow_outline.floor = __instance;
                arrow_outline.renderer.sprite = __instance.isCCW ? Images.arrow_ccw_outline : Images.arrow_cw_outline;
                arrow_outline.renderer.sortingLayerID = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerID;
                arrow_outline.renderer.sortingLayerName = (__instance.iconsprite ?? __instance.floorRenderer.renderer).sortingLayerName;
                arrow_outline.name = "arrow_outline_renderer";
                arrow_outline.transform.localPosition = localPosition;
                arrow_outline.transform.localEulerAngles = new Vector3(0f, 0f, num4 * Mathf.Rad2Deg);
            }
        }
    }
}
