using UnityEngine;

namespace BackToThePast.HideAnnounceSign
{
    public static class HideAnnounceSignTweak
    {
        private static GameObject newsCache;

        public static void ToggleSign(bool show)
        {
            GameObject obj = Object.FindObjectOfType<NewsSign>()?.gameObject;
            if (obj)
            {
                obj.SetActive(show);
                newsCache = obj;
            }
            else if (newsCache)
                newsCache.SetActive(show);
        }
    }
}
