using BackToThePast.Utils;
using UnityEngine;

namespace BackToThePast.HideAnnounceSign
{
    public static class HideAnnounceSignTweak
    {
        public static void ToggleSign(bool show)
        {
            NewsSign[] newsSigns = Object.FindObjectsOfType<NewsSign>();
            foreach (NewsSign newsSign in newsSigns)
            {
                if (newsSign)
                {
                    SpriteRenderer[] renderers = newsSign.Get<SpriteRenderer[]>("spriteRenderers");
                    if (show)
                    {
                        newsSign.text.enabled = true;
                        newsSign.loadingIcon.enabled = true;
                        if (renderers != null)
                            foreach (SpriteRenderer renderer in renderers)
                            {
                                renderer.enabled = true;
                            }
                    }
                    else
                    {
                        newsSign.text.enabled = false;
                        newsSign.loadingIcon.enabled = false;
                        if (renderers != null)
                            foreach (SpriteRenderer renderer in renderers)
                            {
                                renderer.enabled = false;
                            }
                    }
                }
            }
        }
    }
}
