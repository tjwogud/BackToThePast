using UnityEngine;

namespace BackToThePast.LegacyTwirl
{
    public class TwirlRenderer : MonoBehaviour
    {
        public SpriteRenderer renderer;
        public scrFloor floor;
        public bool outline;

        public void Awake()
        {
            renderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        }

        public void LateUpdate()
        {
            if (floor == null)
                return;
            if (floor.floorIcon != FloorIcon.Swirl && floor.floorIcon != FloorIcon.SwirlCW)
                Destroy(this);
            renderer.sortingOrder = (floor.iconsprite ?? floor.floorRenderer.renderer).sortingOrder + (outline ? 1 : 2);
            renderer.transform.localScale = floor.transform.localScale;
            renderer.SetAlpha(floor.floorRenderer.color.a * floor.opacity);
        }
    }
}
