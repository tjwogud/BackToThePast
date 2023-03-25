using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackToThePast.OldBackground
{
    public static class OldBackgroundTweak
    {
        public static void SetBackground()
        {
            if (ADOBase.levelSelect == null)
                return;
            GameObject bg = SceneManager.GetActiveScene().GetRootGameObjects().First(g => g.name == "BG");
            if (!bg)
                return;
            for (int i = 0; i < 3; i++)
            {
                if ((!Main.Settings.oldBackground && i == 2) || (Main.Settings.oldBackground && Main.Settings.oldBackgroundIndex == i))
                {
                    bg.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    bg.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
