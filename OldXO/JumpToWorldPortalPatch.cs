using BackToThePast.Patch;
using BackToThePast.Utils;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scnLevelSelect), "JumpToWorldPortal")]
    public static class JumpToWorldPortalPatch
    {
        public static bool Prefix(string world, ref bool wipeFirst, scnLevelSelect __instance)
        {
            scrPlanet chosenplanet = scrController.instance.chosenplanet;
            float planetX = chosenplanet.transform.position.x;
            float planetY = chosenplanet.transform.position.y;
            Transform transform = chosenplanet.transform;
            if (world == null && planetX == 15 && planetY == 23)
            {
                transform.LocalMoveXY(0, 23);
                return true;
            }
            if ((PlayerPrefs.GetInt("BackToThePast.FoundSecretIsland", 0) == 1 && !wipeFirst && world == null && planetX == 0 && planetY == 23) || world == "BackToThePast.SecretIsland")
            {
                if (wipeFirst)
                    scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                    {
                        __instance.Set("responsive", true);
                        __instance.JumpToWorldPortal(world, true, false);
                        scrUIController.instance.WipeFromBlack();
                        foreach (Collider2D collider in Physics2D.OverlapPointAll(new Vector2(planetX, planetY), 1 << LayerMask.NameToLayer("Floor")))
                        {
                            scrFloor component = collider.GetComponent<scrFloor>();
                            if (component.isLandable)
                                chosenplanet.currfloor = component;
                        }
                    });
                else
                {
                    scrCamera.instance.SetXOffset(0);
                    scrCamera.instance.SetYOffset(0);
                    transform.LocalMoveXY(15, 23);
                    __instance.Set("menuPhase", 1);
                    scrCamera.instance.positionState = (PositionState)1001;
                    scrCamera.instance.isMoveTweening = true;
                    scrCamera.instance.Refocus(transform);
                }
                return false;
            }
            return true;
        }
    }
}
