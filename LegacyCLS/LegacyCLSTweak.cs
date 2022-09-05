using BackToThePast.Utils;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BackToThePast.LegacyCLS
{
    public static class LegacyCLSTweak
    {
        public static InputField searchField;
        public static CanvasGroup searchFieldCanvasGroup;
        public static bool searchMode = false;
        public static Sequence searchSeq;

        public static void CreateInputField()
        {
            if (searchField)
                return;
            GameObject searchFieldContainer = GameObject.Find("Search Field Container");
            if (searchFieldContainer)
            {
                searchField = searchFieldContainer.transform.GetComponentInChildren<InputField>(true);
                searchFieldCanvasGroup = searchFieldContainer.transform.GetComponentInChildren<CanvasGroup>(true);
            }
            else
            {
                searchFieldContainer = new GameObject("Search Field Container");
                {
                    searchFieldContainer.transform.parent = scnCLS.instance.transform;
                    RectTransform rect = searchFieldContainer.GetOrAddComponent<RectTransform>();
                    rect.offsetMax = new Vector2(276.25f, 0);
                    rect.offsetMin = new Vector2(-276.25f, -100.7f);
                    rect.pivot = new Vector2(0.5f, 1);
                    rect.position = new Vector3(960, 540, 2.5098f);
                    rect.localPosition = new Vector3(0, 0, 2.5098f);
                    rect.anchoredPosition3D = new Vector3(0, 0, 2.5098f);
                    rect.sizeDelta = new Vector2(552.5f, 100.7f);
                    searchFieldContainer.GetOrAddComponent<CanvasRenderer>().cullTransparentMesh = false;
                }
                GameObject searchField = new GameObject("Search Field");
                {
                    searchField.transform.parent = searchFieldContainer.transform;
                    {
                        RectTransform rect = searchField.GetOrAddComponent<RectTransform>();
                        rect.position = new Vector3(960, 489.65f, 2.5098f);
                        rect.localPosition = new Vector3(0, -50.35f, 0);
                        rect.offsetMax = new Vector2(276, 50);
                        rect.offsetMin = new Vector2(-276, -50);
                        rect.sizeDelta = new Vector2(552, 100);
                    }
                    searchField.GetOrAddComponent<CanvasRenderer>().cullTransparentMesh = false;
                    Image image = searchField.AddComponent<Image>();
                    image.sprite = Images.editor_button_fill;
                    image.type = Image.Type.Sliced;
                    InputField inputField = searchField.AddComponent<InputField>();
                    inputField.caretColor = new Color(0.1961f, 0.1961f, 0.1961f);
                    inputField.image = image;
                    inputField.targetGraphic = image;
                    LegacyCLSTweak.searchField = inputField;
                    searchFieldCanvasGroup = searchField.AddComponent<CanvasGroup>();
                    GameObject placeholder = new GameObject("Placeholder");
                    {
                        placeholder.transform.parent = searchField.transform;
                        RectTransform rect = placeholder.GetOrAddComponent<RectTransform>();
                        rect.anchoredPosition = new Vector2(0, -0.5f);
                        rect.anchorMax = new Vector2(1, 1);
                        rect.anchorMin = new Vector2(0, 0);
                        rect.offsetMax = new Vector2(-10, -7);
                        rect.offsetMin = new Vector2(10, 6);
                        rect.sizeDelta = new Vector2(-20, -13);
                        placeholder.GetOrAddComponent<CanvasRenderer>().cullTransparentMesh = false;
                        Text text = placeholder.AddComponent<Text>();
                        text.alignByGeometry = false;
                        text.alignment = TextAnchor.MiddleCenter;
                        text.fontSize = 60;
                        text.fontStyle = FontStyle.Italic;
                        text.horizontalOverflow = HorizontalWrapMode.Wrap;
                        text.lineSpacing = 1;
                        text.supportRichText = true;
                        text.verticalOverflow = VerticalWrapMode.Truncate;
                        text.text = RDString.Get("cls.find");
                        text.color = new Color(0.1961f, 0.1961f, 0.1961f, 0.5f);
                        text.SetLocalizedFont();
                        inputField.placeholder = text;
                    }
                    GameObject textObj = new GameObject("Text");
                    {
                        textObj.transform.parent = searchField.transform;
                        RectTransform rect = textObj.GetOrAddComponent<RectTransform>();
                        rect.anchoredPosition = new Vector2(0, -0.5f);
                        rect.anchorMax = new Vector2(1, 1);
                        rect.anchorMin = new Vector2(0, 0);
                        rect.offsetMax = new Vector2(-10, -7);
                        rect.offsetMin = new Vector2(10, 6);
                        rect.sizeDelta = new Vector2(-20, -13);
                        textObj.GetOrAddComponent<CanvasRenderer>().cullTransparentMesh = false;
                        Text text = textObj.AddComponent<Text>();
                        text.alignByGeometry = false;
                        text.alignment = TextAnchor.MiddleCenter;
                        text.fontSize = 60;
                        text.horizontalOverflow = HorizontalWrapMode.Overflow;
                        text.lineSpacing = 1;
                        text.supportRichText = false;
                        text.verticalOverflow = VerticalWrapMode.Truncate;
                        text.text = "";
                        text.color = new Color(0.1961f, 0.1961f, 0.1961f);
                        text.SetLocalizedFont();
                        inputField.textComponent = text;
                    }
                    searchField.SetActive(false);
                }
            }
            searchField.onValueChanged.AddListener(sub => scnCLS.instance.SearchLevels(sub, true));
            searchField.onEndEdit.AddListener(_ => scnCLS.instance.ToggleSearchMode(false));
        }

        public static void ToggleSearchMode(bool search)
        {
            scnCLS.instance.StartCoroutine(ToggleSearchModeCo(search));
        }

        private static IEnumerator ToggleSearchModeCo(bool search)
        {
            scnCLS.instance.lastFrameSearchModeAvailable = Time.frameCount;
            scnCLS.instance.Get("optionsPanels").Set("searchMode", searchMode = search);
            if (search && RDC.runningOnSteamDeck)
                while (!SteamWorkshop.ShowTextInput())
                    yield return null;
            if (searchSeq != null)
                searchSeq.Kill(false);
            float duration = 0.33f;
            searchSeq = DOTween.Sequence().Insert(0f, searchFieldCanvasGroup.DOFade(search ? 1f : 0f, duration).SetEase(Ease.OutCubic)).Insert(0f, searchField.GetComponent<RectTransform>().DOPivotY(search ? 0f : 0.5f, duration).SetEase(Ease.OutCubic));
            if (search)
            {
                searchField.gameObject.SetActive(true);
                scrController.instance.responsive = false;
                searchField.ActivateInputField();
                yield break;
            }
            searchSeq.OnComplete(() => searchField.gameObject.SetActive(false));
            yield return EnableInputCo();
        }

        private static IEnumerator EnableInputCo()
        {
            yield return new WaitForEndOfFrame();
            scrController.instance.responsive = true;
            yield break;
        }

        public static void Toggle(bool active)
        {
            if (scnCLS.instance == null || (!active && !searchField))
                return;
            ADOBase optionsPanels = (ADOBase)scnCLS.instance.Get("optionsPanels");
            if (active)
            {
                scnCLS.instance.gameObject.GetOrAddComponent<WorkshopShortcut>();
                CreateInputField();
                optionsPanels.Method("TogglePanel", new object[] { true, false });
                optionsPanels.Method("TogglePanel", new object[] { false, false });
                searchField.text = scnCLS.instance.Get("optionsPanels").Get<InputField>("searchInputField").text;
            }
            else
            {
                ToggleSearchMode(false);
                InputField inputField = scnCLS.instance.Get("optionsPanels").Get<InputField>("searchInputField");
                inputField.text = searchField.text;
            }
            optionsPanels.gameObject.SetActive(!active);
            scnCLS.instance.transform.Find("LevelInfoCanvas").Find("HelpContainer").gameObject.SetActive(active);
        }
    }
}
