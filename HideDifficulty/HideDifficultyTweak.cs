using BackToThePast.Utils;
using HarmonyLib;

namespace BackToThePast.HideDifficulty
{
    public static class HideDifficultyTweak
    {
        public static void ToggleDifficulty(bool show)
        {
            scrUIController instance = scrUIController.instance;
            scnEditor scnEditor = scnEditor.instance;
            if (show)
            {
                if (scnEditor != null)
                {
                    if (!scnEditor.editorDifficultySelector.gameObject.activeSelf)
                        scnEditor.editorDifficultySelector.gameObject.SetActive(true);
                }
                if (instance != null)
                {
                    if (!instance.difficultyContainer.gameObject.activeSelf)
                        instance.difficultyContainer.gameObject.SetActive(true);
                    if (!instance.difficultyFadeContainer.gameObject.activeSelf)
                        instance.difficultyFadeContainer.gameObject.SetActive(true);
                }
            }
            else
            {
                GCS.difficulty = Difficulty.Strict;
                if (instance != null)
                {
                    if (scnEditor != null)
                    {
                        scnEditor.editorDifficultySelector.Method("UpdateDifficultyDisplay");
                        if (scnEditor.editorDifficultySelector.gameObject.activeSelf)
                            scnEditor.editorDifficultySelector.gameObject.SetActive(false);
                    }
                    if (AccessTools.Field(typeof(scrUIController), "currentDifficultyIndex") != null)
                    {
                        instance.Set("currentDifficultyIndex", 2);
                        instance.Method("UpdateDifficultyUI");
                    }
                    else
                        instance.Method("UpdateDifficultyUI", new object[] { Difficulty.Strict });
                    GCS.difficulty = Difficulty.Strict;
                    if (instance.difficultyContainer.gameObject.activeSelf)
                        instance.difficultyContainer.gameObject.SetActive(false);
                    if (instance.difficultyFadeContainer.gameObject.activeSelf)
                        instance.difficultyFadeContainer.gameObject.SetActive(false);
                }
            }
        }
    }
}
