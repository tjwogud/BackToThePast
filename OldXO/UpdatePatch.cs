using HarmonyLib;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [HarmonyPatch(typeof(scrCamera), "Update")]
    public static class UpdatePatch
    {
        public static void Prefix()
        {
            if ((int)scrCamera.instance.positionState == 1001)
                scrCamera.instance.topos = new Vector3(scrCamera.instance.topos.x, 24, -10);
        }
    }
}
