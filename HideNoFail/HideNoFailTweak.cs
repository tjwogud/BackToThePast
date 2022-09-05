using UnityEngine;
using UnityEngine.UI;

namespace BackToThePast.HideNoFail
{
    public static class HideNoFailTweak
    {
        public static void ToggleNoFail(bool show)
        {
            scnEditor scnEditor = scnEditor.instance;
            if (show)
            {
                if (scrController.instance != null && scnEditor != null)
                    scnEditor.buttonNoFail.gameObject.SetActive(true);
            }
            else
            {
                GCS.useNoFail = false;
                if (scrController.instance != null)
                {
                    scrController.instance.noFail = false;
                    if (scnEditor != null)
                    {
                        scnEditor.buttonNoFail.GetComponent<Image>().color = new Color(0.42352942f, 0.42352942f, 0.42352942f);
                        scnEditor.buttonNoFail.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
