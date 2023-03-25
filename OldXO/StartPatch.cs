using ADOFAI.ModdingConvenience;
using BackToThePast.Patch;
using BackToThePast.Utils;
using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scnLevelSelect), "Start")]
    public static class StartPatch
    {
        public static void Postfix()
        {
            if (Persistence.GetSavedCurrentLevel().StartsWith("BackToThePast.OldXO"))
                Object.FindObjectOfType<scrMenuContinueInfo>().GetComponent<Text>().text = $"({Main.Localization["bttp.oldxo.title"]})";
            GameObject oldXO = new GameObject("OldXO");
            for (int i = 0; i < 15; i++)
                if (i == 2)
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, () => scrCamera.instance.positionState = PositionState.CrownIsland, oldXO.transform).gameObject.AddComponent<FloorHider>();
                else if (i == 3)
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, () => scrCamera.instance.positionState = (PositionState)1001, oldXO.transform).gameObject.AddComponent<FloorHider>();
                else if (i == 10)
                    FloorUtils.AddEventFloor(1, 0, 5 + i, 23, () => {
                        if (PlayerPrefs.GetInt("BackToThePast.FoundSecretIsland", 0) == 0)
                        {
                            PlayerPrefs.SetInt("BackToThePast.FoundSecretIsland", 1);
                            PlayerPrefs.Save();
                        }
                    }, oldXO.transform);
                else if (i > 10)
                    FloorUtils.AddFloor(5 + i, 23, oldXO.transform);
                else
                    FloorUtils.AddEventFloor(0, -3, 5 + i, 23, null, oldXO.transform).gameObject.AddComponent<FloorHider>();
            scrFloor normal = FloorUtils.AddEventFloor(0, -3, 18, 30, () => OldXOTweak.EnterLevel(false, false), oldXO.transform);
            normal.isportal = true;
            normal.floorRenderer.sortingOrder = 0;
            scrPortalParticles normalParticle = Object.Instantiate(PrefabLibrary.instance.lastTilePortalPrefab, normal.transform);
            normalParticle.Method("Start");
            Object.Destroy(normalParticle);
            scrFloor speed = FloorUtils.AddEventFloor(0, -3, 20, 30, () => OldXOTweak.EnterLevel(true, false), oldXO.transform);
            speed.isportal = true;
            speed.floorRenderer.sortingOrder = 0;
            scrPortalParticles speedParticle = Object.Instantiate(PrefabLibrary.instance.lastTilePortalPrefab, speed.transform);
            speedParticle.speedTrial = true;
            speedParticle.Method("Start");
            Object.Destroy(speedParticle);
            scrFloor portal = FloorUtils.AddEventFloor(0, -3, 19, 30, () =>
            {
                scrCamera.instance.positionState = (PositionState)1002;
                DOTween.TweensById("BackToThePast.OldXO.normal")?.ForEach(t => t.Kill(false));
                DOTween.TweensById("BackToThePast.OldXO.speed")?.ForEach(t => t.Kill(false));
                normal.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.normal");
                speed.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.speed");
            }, oldXO.transform);
            portal.iconsprite = FloorUtils.GetFloorGameObjectAt(-5, 27).GetComponent<scrFloor>().iconsprite;
            FloorUtils.AddEventFloor(1, 0, 19, 24, () => {
                DOTween.TweensById("BackToThePast.OldXO.portal")?.ForEach(t => t.Kill(false));
                portal.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.portal");
            }, oldXO.transform);
            FloorUtils.AddEventFloor(1, 0, 19, 25, () => {
                scrCamera.instance.positionState = (PositionState)1001;
                DOTween.TweensById("BackToThePast.OldXO.portal")?.ForEach(t => t.Kill(false));
                DOTween.TweensById("BackToThePast.OldXO.normal")?.ForEach(t => t.Kill(false));
                DOTween.TweensById("BackToThePast.OldXO.speed")?.ForEach(t => t.Kill(false));
                portal.transform.DOMoveY(26, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.portal");
                normal.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.normal");
                speed.transform.DOMoveY(30, 1).SetEase(Ease.OutCubic).SetId("BackToThePast.OldXO.speed");
            }, oldXO.transform);
            foreach (scrFloor floor in oldXO.GetComponentsInChildren<scrFloor>())
            {
                floor.Set("oriScaleBottomGlow", 0.26f / 5);
                floor.Set("oriScaleTopGlow", 0.32f / 4);
            }

            object portals = null;
            if (typeof(scrPortal).Get("portals").GetType().FullName.Contains("List"))
                portals = typeof(scrPortal).Get<List<scrPortal>>("portals").ToList();
            oldXO.SetActive(false);
            scrPortal portalComp = Object.Instantiate(RDConstants.data.prefab_worldPortal, oldXO.transform).GetComponent<scrPortal>();
            portalComp.world = "BackToThePast.OldXO";
            oldXO.SetActive(true);
            portalComp.transform.position = new Vector2(19, 29);
            Texture2D texture = Resources.Load<Texture2D>("portalimages/xo");
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 24.4185f);
            portalComp.sprPortal.sprite = sprite;
            if (typeof(scrPortal).Get("portals").GetType().FullName.Contains("List"))
                typeof(scrPortal).Set("portals", portals);

            Transform info = Object.Instantiate(((Component)Resources.FindObjectsOfTypeAll(typeof(scrGfxFloat)).First(comp => comp.name == "InfoXO")).transform.GetChild(0), oldXO.transform);
            info.localPosition = new Vector2(22, 28.5f);
            info.rotation = Quaternion.identity;
            Transform infoCanvas = info.GetChild(0);
            infoCanvas.localPosition = Vector3.zero;
            Transform infoCanvasChild1 = infoCanvas.GetChild(1);
            infoCanvasChild1.localPosition = Vector3.zero;
            infoCanvasChild1.rotation = Quaternion.identity;
            Transform infoCanvasChild2 = infoCanvas.GetChild(0);
            infoCanvasChild2.localPosition = new Vector2(0, 440);
            infoCanvasChild2.rotation = Quaternion.identity;
        }
    }
}
