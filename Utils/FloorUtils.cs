using ADOFAI.ModdingConvenience;
using ByteSheep.Events;
using UnityEngine;

namespace BackToThePast.Utils
{
    public static class FloorUtils
    {
        public static scrFloor AddFloor(float x, float y, Transform parent = null)
        {
            var obj = CreateFloor(parent);
            obj.transform.position = new Vector3(x, y);
            return obj;
        }

        public static scrFloor AddEventFloor(float x, float y, QuickAction action, bool gem, Transform parent = null)
        {
            scrFloor obj;
            if (gem)
            {
                GameObject origin = GameObject.Find("Floors").transform.Find("Grid").Find("outer ring").Find("ChangingRoomGem").Find("MovingGem").gameObject;
                obj = Object.Instantiate(origin, parent).GetComponent<scrFloor>();
                obj.transform.position = new Vector3(x, y);
                Object.Destroy(obj.GetComponent<scrDisableIfWorldNotComplete>());
                Object.Destroy(obj.GetComponent<scrMenuMovingFloor>());
                scrGem scrGem = obj.gameObject.GetComponent<scrGem>();
                scrGem.startPosition = scrGem.endPosition = null;
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = AddFloor(x, y, parent);
                if (!obj)
                    return null;
            }
            var func = obj.gameObject.GetOrAddComponent<ffxCallFunction>();
            if (func.ue == null)
            {
                func.ue = new QuickEvent();
                func.ue.persistentCalls = new QuickPersistentCallGroup();
            }
            func.ue.RemoveAllListeners();
            func.ue.AddListener(action);
            func.enabled = true;
            return obj;
        }

        public static scrFloor AddTeleportFloor(float x, float y, float targetX, float targetY, float cameraX, float cameraY, bool cameraMoving = true, PositionState state = PositionState.None, QuickAction action1 = null, QuickAction action2 = null, Transform parent = null)
        {
            return AddEventFloor(x, y, delegate
            {
                if (action1 != null)
                    action1.Invoke();
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    if (action2 != null)
                        action2.Invoke();
                    scrController.instance.chosenplanet.transform.LocalMoveXY(targetX, targetY);
                    scrController.instance.chosenplanet.transform.position = new Vector3(targetX, targetY);
                    scrController.instance.camy.ViewObjectInstant(scrController.instance.chosenplanet.transform, false);
                    scrController.instance.camy.ViewVectorInstant(new Vector2(cameraX, cameraY), false);
                    scrController.instance.camy.isMoveTweening = cameraMoving;
                    scrController.instance.camy.positionState = state;
                    scrUIController.instance.WipeFromBlack();
                    scrFloor component = GetFloorGameObjectAt(targetX, targetY).GetComponent<scrFloor>();
                    scrController.instance.chosenplanet.currfloor = component;
                });
            }, parent);
        }

        public static scrFloor CreateFloor(Transform parent = null)
        {
            return Object.Instantiate(PrefabLibrary.instance.scnLevelSelectFloorPrefab.gameObject, parent).GetComponent<scrFloor>();
        }

        public static GameObject GetFloorGameObjectAt(float x, float y)
        {
            var array = Physics2D.OverlapPointAll(new Vector2(x, y), 1 << LayerMask.NameToLayer("Floor"));
            return array.Length == 0 ? null : array[0].gameObject;
        }
    }
}
