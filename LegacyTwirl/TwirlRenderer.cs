using UnityEngine;

namespace BackToThePast.LegacyTwirl
{
    public class TwirlRenderer : MonoBehaviour
    {
        public SpriteRenderer renderer;
        public scrFloor floor;

        public void Awake()
        {
            renderer = gameObject.AddComponent<SpriteRenderer>();
        }

        public void LateUpdate()
        {
            if (floor == null)
                return;
            renderer.transform.localScale = floor.transform.localScale;
            renderer.SetAlpha(floor.floorRenderer.color.a * floor.opacity);
        }
    }
}
