using UnityEngine;
using UnityEngine.UI;

namespace BackToThePast.LegacyEditorButtons
{
    public static class LegacyEditorButtonsTweak
    {
        private static bool initialized = false;

        private static (float, float) originalAutoPosition;
        private static (float, float) originalNofailPosition;
        private static (float, float) originalDifficultyPosition;

        private static (float, float) originalAutoOffsetMax;
        private static (float, float) originalNofailOffsetMax;
        private static (float, float) originalDifficultyOffsetMax;

        private static (float, float) originalAutoOffsetMin;
        private static (float, float) originalNofailOffsetMin;
        private static (float, float) originalDifficultyOffsetMin;

        private static (float, float) originalNofailPivot;
        private static (float, float, float, float) originalNofailRect;

        public static void ChangeEditorButtons(bool change)
        {
            scnEditor scnEditor = scnEditor.instance;
            if (scnEditor == null)
                return;
            RectTransform auto = scnEditor.autoImage.GetComponent<RectTransform>();
            RectTransform nofail = scnEditor.buttonNoFail.GetComponent<RectTransform>();
            RectTransform difficulty = scnEditor.editorDifficultySelector.GetComponent<RectTransform>();
            if (change)
            {
                if (!initialized)
                {
                    originalAutoPosition = (auto.offsetMin.x, auto.offsetMin.y);
                    originalNofailPosition = (nofail.offsetMin.x, nofail.offsetMin.y);
                    originalDifficultyPosition = (difficulty.offsetMin.x, difficulty.offsetMin.y);

                    originalAutoOffsetMax = (auto.offsetMax.x, auto.offsetMax.y);
                    originalNofailOffsetMax = (nofail.offsetMax.x, nofail.offsetMax.y);
                    originalDifficultyOffsetMax = (difficulty.offsetMax.x, difficulty.offsetMax.y);

                    originalAutoOffsetMin = (auto.offsetMin.x, auto.offsetMin.y);
                    originalNofailOffsetMin = (nofail.offsetMin.x, nofail.offsetMin.y);
                    originalDifficultyOffsetMin = (difficulty.offsetMin.x, difficulty.offsetMin.y);

                    originalNofailPivot = (nofail.pivot.x, nofail.pivot.y);
                    originalNofailRect = (nofail.rect.x, nofail.rect.y, nofail.rect.width, nofail.rect.height);
                    initialized = true;
                }
                nofail.pivot = new Vector2(0.5f, 0.5f);
                Rect rect = nofail.rect;
                rect.x = -29.5f;
                rect.y = -30;
                rect.width = 59;
                rect.height = 60;

                auto.anchoredPosition = new Vector2(-15, 15);
                nofail.anchoredPosition = new Vector2(-52, 125);
                difficulty.anchoredPosition = new Vector2(-15, 10);

                auto.offsetMax = new Vector2(-15, 95);
                nofail.offsetMax = new Vector2(-22.5f, 155);
                difficulty.offsetMax = new Vector2(-15, 118);

                auto.offsetMin = new Vector2(-95, 15);
                nofail.offsetMin = new Vector2(-81.5f, 95);
                difficulty.offsetMin = new Vector2(-200, 10);
            }
            else
            {
                if (!initialized)
                    return;
                nofail.pivot = new Vector2(originalNofailPivot.Item1, originalNofailPivot.Item2);
                Rect rect = nofail.rect;
                rect.x = originalNofailRect.Item1;
                rect.y = originalNofailRect.Item2;
                rect.width = originalNofailRect.Item3;
                rect.height = originalNofailRect.Item4;

                auto.anchoredPosition = new Vector2(originalAutoPosition.Item1, originalAutoPosition.Item2);
                nofail.anchoredPosition = new Vector2(originalNofailPosition.Item1, originalNofailPosition.Item2);
                difficulty.anchoredPosition = new Vector2(originalDifficultyPosition.Item1, originalDifficultyPosition.Item2);

                auto.offsetMax = new Vector2(originalAutoOffsetMax.Item1, originalAutoOffsetMax.Item2);
                nofail.offsetMax = new Vector2(originalNofailOffsetMax.Item1, originalNofailOffsetMax.Item2);
                difficulty.offsetMax = new Vector2(originalDifficultyOffsetMax.Item1, originalDifficultyOffsetMax.Item2);

                auto.offsetMin = new Vector2(originalAutoOffsetMin.Item1, originalAutoOffsetMin.Item2);
                nofail.offsetMin = new Vector2(originalNofailOffsetMin.Item1, originalNofailOffsetMin.Item2);
                difficulty.offsetMin = new Vector2(originalDifficultyOffsetMin.Item1, originalDifficultyOffsetMin.Item2);
            }
        }

        public static void RemoveShadowAddOutline(bool rsao)
        {
            scnEditor scnEditor = scnEditor.instance;
            if (scnEditor == null)
                return;
            GameObject auto = scnEditor.autoImage.gameObject;
            GameObject nofail = scnEditor.buttonNoFail.gameObject;
            if (auto.GetComponent<Shadow>() is Outline)
                return;
            if (rsao)
            {
                auto.GetComponent<Shadow>().enabled = false;
                auto.GetOrAddComponent<Outline>().effectColor = Color.black;
                nofail.GetComponent<Shadow>().enabled = false;
                nofail.GetOrAddComponent<Outline>().effectColor = Color.black;
            }
            else
            {
                UnityEngine.Object.Destroy(auto.GetComponent<Outline>());
                auto.GetComponent<Shadow>().enabled = true;
                UnityEngine.Object.Destroy(nofail.GetComponent<Outline>());
                nofail.GetComponent<Shadow>().enabled = true;
            }
        }
    }
}
