using BackToThePast.Utils;
using DG.Tweening;
using HarmonyLib;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [HarmonyPatch(typeof(scnLevelSelect), "Start")]
    public static class StartPatch
    {
        public static void Postfix(scnLevelSelect __instance)
        {
            GameObject oldXO = new GameObject("oldXO");
            oldXO.transform.parent = GameObject.Find("Floor Container").transform;
            for (int i = 0; i < 15; i++)
                if (i == 2)
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, () => scrCamera.instance.positionState = PositionState.CrownIsland, oldXO.transform).gameObject.AddComponent<FloorHider>();
                else if (i == 3)
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, () => scrCamera.instance.positionState = (PositionState)1001, oldXO.transform).gameObject.AddComponent<FloorHider>();
                else if (i >= 10)
                    FloorUtils.AddFloor(5 + i, 23, oldXO.transform);
                else
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, null, oldXO.transform).gameObject.AddComponent<FloorHider>();
            scrFloor normal = FloorUtils.AddEventFloor(0, -3, 18, 30, () =>
            {
                GCS.sceneToLoad = "scnEditor";
                GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
                GCS.speedTrialMode = false;
                GCS.practiceMode = false;
                GCS.standaloneLevelMode = true;
                Persistence.SetSavedCurrentLevel("BackToThePast.OldXO.default");
            }, oldXO.transform);
            normal.isportal = true;
            scrFloor speed = FloorUtils.AddEventFloor(0, -3, 20, 30, () =>
            {
                GCS.sceneToLoad = "scnEditor";
                GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
                GCS.speedTrialMode = true;
                GCS.practiceMode = false;
                GCS.standaloneLevelMode = true;
                Persistence.SetSavedCurrentLevel("BackToThePast.OldXO.speedTrial");
            }, oldXO.transform);
            speed.isportal = true;
            speed.SetTileColor(Color.red);
            scrFloor portal = FloorUtils.AddEventFloor(0, -3, 19, 30, () =>
            {
                normal.transform.DOKill(false);
                speed.transform.DOKill(false);
                normal.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic);
                speed.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic);
            }, oldXO.transform);
            portal.iconsprite = FloorUtils.GetFloorGameObjectAt(-5, 27).GetComponent<scrFloor>().iconsprite;
            FloorUtils.AddEventFloor(1, 0, 19, 24, () => {
                portal.transform.DOKill(false);
                portal.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic);
            }, oldXO.transform);
            FloorUtils.AddEventFloor(1, 0, 19, 25, () => {
                portal.transform.DOKill(false);
                normal.transform.DOKill(false);
                speed.transform.DOKill(false);
                portal.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic);
                normal.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic);
                speed.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic);
            }, oldXO.transform);
            foreach (scrFloor floor in oldXO.GetComponentsInChildren<scrFloor>())
            {
                floor.Set("oriScaleBottomGlow", 0.26f / 5);
                floor.Set("oriScaleTopGlow", 0.32f / 4);
            }
        }
    }
}
