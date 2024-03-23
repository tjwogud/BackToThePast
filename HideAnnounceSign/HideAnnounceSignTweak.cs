using BackToThePast.Utils;
using UnityEngine;
using UnityEngine.UI;

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
                    if (!newsSign.button.button)
                        newsSign.button.button = newsSign.button.GetComponent<Button>();
                    newsSign.button.transform.parent.gameObject.SetActive(show);
                    SpriteRenderer[] renderers = newsSign.Get<SpriteRenderer[]>("spriteRenderers");
                    if (renderers != null)
                        foreach (SpriteRenderer renderer in renderers)
                        {
                            renderer.enabled = show;
                        }
                }
            }
        }
    }
}
