using BackToThePast.Patch;
using DG.Tweening;
using UnityEngine;

namespace BackToThePast.JudgementText
{
    [BTTPPatch(typeof(scrHitTextMesh), "Show")]
    public static class ShowPatch
    {
        public static void Postfix(scrHitTextMesh __instance, Renderer ___meshRenderer)
        {
            if (Main.Settings.noJudgeAnimation)
            {
                __instance.transform.DOKill();
                __instance.transform.localRotation = scrCamera.instance.transform.rotation;
                DOTween.TweensByTarget(___meshRenderer.material)[0].SetEase(Ease.InExpo);
            }
        }
    }
}
