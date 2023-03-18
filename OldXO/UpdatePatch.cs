using BackToThePast.Patch;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrCamera), "Update")]
    public static class UpdatePatch
    {
        public static void Prefix()
        {
            switch ((int)scrCamera.instance.positionState)
            {
                case 1001:
                    scrCamera.instance.topos = new Vector3(scrCamera.instance.topos.x, 24, -10);
                    break;
                case 1002:
                    scrCamera.instance.topos = new Vector3(19, 28, -10);
                    break;
            }
        }
    }
}
