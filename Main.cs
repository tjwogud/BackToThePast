using BackToThePast.Utils;
using GDMiniJSON;
using HarmonyLib;
using Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityModManagerNet;

namespace BackToThePast
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static UnityModManager.ModEntry ModEntry;
        public static Settings Settings;
        public static Localization Localization;
        public static FontData legacyFont;
        public static FontData font;
        public static Dictionary<string, object> old_xo;
        public static AudioClip one_forgotten_night;
        public static bool lucky;
        private static readonly PropertyInfo isEditingLevelProperty = typeof(ADOBase).GetProperty("isEditingLevel", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        private static readonly bool adoBaseStatic = isEditingLevelProperty.GetGetMethod().IsStatic;
        public static bool isEditingLevel => (bool)isEditingLevelProperty.GetValue(adoBaseStatic ? null : scrController.instance);

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            Logger.Log("Loading Settings...");
            Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            Logger.Log("Load Completed!");
            Logger.Log("Loading AssetBundle...");
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(modEntry.Path, "backtothepast"));
            if (bundle == null)
                throw new Exception("can't load assetbundle!");
            Images.Load(bundle);
            legacyFont = new FontData() {
                font = bundle.LoadAsset<Font>("Same_Mistake - Kopie"),
                fontScale = 0.95f,
                lineSpacing = 1.45f
            };
            old_xo = (Dictionary<string, object>)Json.Deserialize(bundle.LoadAsset<TextAsset>("old_xo").text);
            one_forgotten_night = bundle.LoadAsset<AudioClip>("One forgotten night");
            Logger.Log("Load Completed!");
            Localization = Localization.Load(modEntry, "1QcrRL6LAs8WxJj_hFsEJa3CLM5g3e8Ya0KQlRKXwdlU", 343830105);
            lucky = new System.Random().Next(20) == 0;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                LegacyResult.OnLandOnPortalPatch.Patch(Settings.legacyResult);
                RDString.initialized = false;
                SceneManager.activeSceneChanged += OnChangeScene;
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
                LegacyResult.OnLandOnPortalPatch.Patch(false);
                SceneManager.activeSceneChanged -= OnChangeScene;
            }
            return true;
        }

        private static void OnChangeScene(Scene current, Scene next)
        {
            if (Settings.hideDifficulty)
                HideDifficulty();
            if (Settings.hideNoFail)
                HideNoFail();
        }

        public static void HideDifficulty()
        {
            GCS.difficulty = Difficulty.Strict;
            scrUIController instance = scrUIController.instance;
            scnEditor scnEditor = scnEditor.instance;
            if (scrController.instance != null && isEditingLevel && scnEditor != null)
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
                } else
                    instance.Method("UpdateDifficultyUI", new object[] { Difficulty.Strict });
                GCS.difficulty = Difficulty.Strict;
                if (instance.difficultyContainer.gameObject.activeSelf == true)
                    instance.difficultyContainer.gameObject.SetActive(false);
                if (instance.difficultyFadeContainer.gameObject.activeSelf == true)
                    instance.difficultyFadeContainer.gameObject.SetActive(false);
            }
        }

        public static void ShowDifficulty()
        {
            scrUIController instance = scrUIController.instance;
            scnEditor scnEditor = scnEditor.instance;
            if (isEditingLevel && scnEditor != null)
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

        public static void HideNoFail()
        {
            GCS.useNoFail = false;
            scnEditor scnEditor = scnEditor.instance;
            if (scrController.instance != null)
            {
                scrController.instance.noFail = false;
                if (isEditingLevel && scnEditor != null)
                {
                    scnEditor.buttonNoFail.GetComponent<Image>().color = new Color(0.42352942f, 0.42352942f, 0.42352942f);
                    if (scnEditor.buttonNoFail.gameObject.activeSelf == true)
                        scnEditor.buttonNoFail.gameObject.SetActive(false);
                }
            }
        }

        public static void ShowNoFail()
        {
            scnEditor scnEditor = scnEditor.instance;
            if (scrController.instance != null && isEditingLevel && scnEditor != null && scnEditor.buttonNoFail.gameObject.activeSelf == false)
                scnEditor.buttonNoFail.gameObject.SetActive(true);
        }

        private static bool inited = false;

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
                if (!inited)
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
                    inited = true;
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
                if (!inited)
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

        private static bool initialized = false;

        private static GUIStyle label;
        private static GUIStyle btn;
        private static bool play = false;
        private static bool editor = false;
        private static bool sfx = false;
        private static bool etc = false;

        public static FontData LegacyFont { get => legacyFont; set => legacyFont = value; }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (Localization.Failed)
            {
                GUILayout.Label("Can't load localizations! Try restart the game.");
                return;
            }
            if (!Localization.Loaded)
            {
                GUILayout.Label("Localizations are not loaded yet! Please wait...");
                return;
            }
            if (!initialized)
            {
                initialized = true;
                label = new GUIStyle(GUI.skin.label);
                label.fontSize = 18;
                btn = new GUIStyle(GUI.skin.button);
                btn.fontSize = 16;
            }

            play = GUILayout.Toggle(play,
                $"{(play ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.play"]}",
                label);
            if (play)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting(() => Settings.legacyResult, b => Settings.legacyResult = b, "legacyResult", LegacyResult.OnLandOnPortalPatch.Patch);
                ShowSetting(() => Settings.noResult, b => Settings.noResult = b, "noResult");
                ShowSetting(() => Settings.hideDifficulty, b => Settings.hideDifficulty = b, "hideDifficulty", c => {
                    if (c)
                        HideDifficulty();
                    else
                        ShowDifficulty();
                });
                ShowSetting(() => Settings.hideNoFail, b => Settings.hideNoFail = b, "hideNoFail", c => {
                    if (c)
                        HideNoFail();
                    else
                        ShowNoFail();
                });
                ShowSetting(() => Settings.oldPracticeMode, b => Settings.oldPracticeMode = b, "oldPracticeMode");
                ShowSetting(() => Settings.showSmallSpeedChange, b => Settings.showSmallSpeedChange = b, "showSmallSpeedChange");
                ShowSetting(() => Settings.legacyFlash, b => Settings.legacyFlash = b, "legacyFlash");
                ShowSetting(() => Settings.noJudgeAnimation, b => Settings.noJudgeAnimation = b, "noJudgeAnimation");
                ShowSetting(() => Settings.lateJudgement, b => Settings.lateJudgement = b, "lateJudgement");
                ShowSetting(() => Settings.forceJudgeCount, b => Settings.forceJudgeCount = b, "forceJudgeCount");
                if (Settings.forceJudgeCount)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowIntSlider(() => Settings.judgeCount, i => Settings.judgeCount = i, 1, 100);
                    GUILayout.EndHorizontal();
                }
                ShowSetting(() => Settings.legacyTwirl, b => Settings.legacyTwirl = b, "legacyTwirl", c => scnEditor.instance?.ApplyEventsToFloors());
                if (Settings.legacyTwirl)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowSetting(() => Settings.twirlWithoutArrow, b => Settings.twirlWithoutArrow = b, "twirlWithoutArrow", c => scnEditor.instance?.ApplyEventsToFloors());
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            editor = GUILayout.Toggle(editor,
                $"{(editor ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.editor"]}",
                label);
            if (editor)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting(() => Settings.space360Tile, b => Settings.space360Tile = b, "space360Tile");
                ShowSetting(() => Settings.weakAuto, b => Settings.weakAuto = b, "weakAuto");
                ShowSetting(() => Settings.whiteAuto, b => Settings.whiteAuto = b, "whiteAuto");
                ShowSetting(() => Settings.legacyEditorButtonsPositions, b => Settings.legacyEditorButtonsPositions = b, "legacyEditorButtonsPositions", ChangeEditorButtons);
                ShowSetting(() => Settings.legacyEditorButtonsDesigns, b => Settings.legacyEditorButtonsDesigns = b, "legacyEditorButtonsDesigns", RemoveShadowAddOutline);
                if (RDString.language == SystemLanguage.Korean)
                    ShowSetting(() => Settings.legacyTexts, b => Settings.legacyTexts = b, "legacyTexts", c =>
                    {
                        if (scnEditor.instance != null)
                        {
                            RDString.initialized = false;
                            Persistence.Save();
                            ADOBase.RestartScene();
                        }
                    });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            sfx = GUILayout.Toggle(sfx,
                $"{(sfx ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.sfx"]}",
                label);
            if (sfx)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting(() => Settings.disablePurePerfectSound, b => Settings.disablePurePerfectSound = b, "disablePurePerfectSound");
                ShowSetting(() => Settings.disableWindSound, b => Settings.disableWindSound = b, "disableWindSound");
                ShowSetting(() => !GCS.playDeathSound, b => GCS.playDeathSound = !b, "disableDeathSound");
                ShowSetting(() => Settings.disableCountdownSound, b => Settings.disableCountdownSound = b, "disableCountdownSound");
                ShowSetting(() => Settings.disableEndingSound, b => Settings.disableEndingSound = b, "disableEndingSound");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            etc = GUILayout.Toggle(etc,
                $"{(etc ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.etc"]}",
                label);
            if (etc)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting(() => Settings.legacyFont, b => Settings.legacyFont = b, "legacyFont", c =>
                {
                    RDString.initialized = false;
                    Persistence.Save();
                    ADOBase.RestartScene();
                });
                if (Settings.legacyFont)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    ShowSetting(() => Settings.butNotJudgement, b => Settings.butNotJudgement = b, "butNotJudgement", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting(() => Settings.butNotCountdown, b => Settings.butNotCountdown = b, "butNotCountdown", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting(() => Settings.butNotTitle, b => Settings.butNotTitle = b, "butNotTitle", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting(() => Settings.butNotSetting, b => Settings.butNotSetting = b, "butNotSetting", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                ShowSetting(() => Settings.disableAlphaWarning, b => Settings.disableAlphaWarning = b, "disableAlphaWarning");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            if (!lucky)
                return;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("☆★==>", label);
            if (GUILayout.Button(RDString.language == SystemLanguage.Korean ? "1972년 11월 21일 전으로가기" : "back to the REAL past", btn))
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://fizzd.itch.io/a-dance-of-fire-and-ice",
                    UseShellExecute = true
                });
            GUILayout.Label("<==★☆", label);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void ShowSetting(Func<bool> getter, Action<bool> setter, string key, Action<bool> onChange = null)
        {
            bool prev = getter.Invoke();
            bool current = GUILayout.Toggle(prev,
                     $"{(prev ? "☑" : "☐")} " +
                     $"{Localization[$"bttp.settings.{key}"]}",
                     label);
            setter.Invoke(current);
            if (prev != current)
                onChange?.Invoke(current);
        }

        private static void ShowIntSlider(Func<int> getter, Action<int> setter, int min, int max, Action<int> onChange = null)
        {
            ShowSlider(() => getter.Invoke(), f => setter.Invoke((int)f), min, max, 0, f => onChange.Invoke((int)f));
        }

        private static void ShowSlider(Func<float> getter, Action<float> setter, float min, float max, int decimals = -1, Action<float> onChange = null)
        {
            float prev = getter.Invoke();
            GUILayout.BeginHorizontal();
            float current = GUILayout.HorizontalSlider(prev, min, max, GUILayout.Width(200));
            if (decimals > -1)
                current = Mathf.Round(current * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
            GUILayout.Label($"{current}", label);
            GUILayout.EndHorizontal();
            setter.Invoke(current);
            if (prev != current)
                onChange?.Invoke(current);
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Settings...");
            Settings.Save(modEntry);
            Logger.Log("Save Completed!");
        }
    }
}
