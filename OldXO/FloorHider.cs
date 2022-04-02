using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace BackToThePast.OldXO
{
    public class FloorHider : MonoBehaviour
    {
        private scrFloor floor;
        private bool prev_hide;
        private Tween tween;

        public void Awake()
        {
            floor = GetComponent<scrFloor>();
            floor.floorRenderer.color = floor.floorRenderer.color.WithAlpha(0);
            floor.topglow.color = floor.topglow.color.WithAlpha(0);
            floor.bottomglow.color = floor.bottomglow.color.WithAlpha(0);
            floor.opacity = 0;
            prev_hide = true;
            if (!floor)
            {
                Main.Logger.Log("No scrFloor!");
                Destroy(this);
            }
        }

        public void Update()
        {
            if (!scrController.instance)
                return;
            bool hide = scrController.instance.chosenplanet.currfloor != floor && !ContainsMouse;
            if (hide && !prev_hide)
            {
                tween?.Kill(false);
                tween = DOTween.To(() => floor.opacity, a =>
                {
                    floor.floorRenderer.color = floor.floorRenderer.color.WithAlpha(a);
                    floor.topglow.color = floor.topglow.color.WithAlpha(a);
                    floor.bottomglow.color = floor.bottomglow.color.WithAlpha(a);
                    floor.opacity = a;
                }, 0, 0.5f).SetEase(Ease.OutQuad);
            }
            else if (!hide && prev_hide)
            {
                tween?.Kill(false);
                tween = DOTween.ToAlpha(() => floor.floorRenderer.color, c =>
                {
                    floor.floorRenderer.color = c;
                    floor.topglow.color = c;
                    floor.bottomglow.color = c;
                    floor.opacity = c.a;
                }, 1, 0.5f).SetEase(Ease.OutQuad);
            }
            prev_hide = hide;
        }

        public bool ContainsMouse => Physics2D.RaycastAll(scrCamera.instance.camobj.ScreenToWorldPoint(Input.mousePosition).xy(), Vector2.zero, 0f).Select(r => r.collider.gameObject).Contains(floor.gameObject);
    }
}
