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
                    if (scnEditor.editorDifficultySelector.gameObject.activeSelf == false)
                        scnEditor.editorDifficultySelector.gameObject.SetActive(true);
                }
                else if (instance != null)
                {
                    if (instance.difficultyContainer.gameObject.activeSelf == false)
                        instance.difficultyContainer.gameObject.SetActive(true);
                    if (instance.difficultyFadeContainer.gameObject.activeSelf == false)
                        instance.difficultyFadeContainer.gameObject.SetActive(true);
                }
            }
            else
            {
                GCS.difficulty = Difficulty.Strict;
                if (scnEditor != null)
                {
                    scnEditor.editorDifficultySelector.Method("UpdateDifficultyDisplay");
                    if (scnEditor.editorDifficultySelector.gameObject.activeSelf == true)
                        scnEditor.editorDifficultySelector.gameObject.SetActive(false);
                }
                else if (instance != null)
                {
                    if (AccessTools.Field(typeof(scrUIController), "currentDifficultyIndex") != null)
                    {
                        instance.Set("currentDifficultyIndex", 2);
                        instance.Method("UpdateDifficultyUI");
                    }
                    else
                        instance.Method("UpdateDifficultyUI", new object[] { Difficulty.Strict });
                    GCS.difficulty = Difficulty.Strict;
                    if (instance.difficultyContainer.gameObject.activeSelf == true)
                        instance.difficultyContainer.gameObject.SetActive(false);
                    if (instance.difficultyFadeContainer.gameObject.activeSelf == true)
                        instance.difficultyFadeContainer.gameObject.SetActive(false);
                }
            }
        }
    }
}
