using BackToThePast.Utils;
using GDMiniJSON;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public static UnityModManager.ModEntry modEntry;
        public static Settings Settings;
        public static FontData legacyFont;
        public static FontData font;
        public static Dictionary<string, object> old_xo;
        public static AudioClip one_forgotten_night;
        public static bool lucky;
        private static PropertyInfo isEditingLevelProperty = typeof(ADOBase).GetProperty("isEditingLevel", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        private static bool adoBaseStatic = isEditingLevelProperty.GetGetMethod().IsStatic;
        public static bool isEditingLevel => (bool)isEditingLevelProperty.GetValue(adoBaseStatic ? null : scnEditor.instance);

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Main.modEntry = modEntry;
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
                scrUIController.instance?.Set("currentDifficultyIndex", 2);
                scrUIController.instance?.Method("UpdateDifficultyUI");
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
                $"{(RDString.language == SystemLanguage.Korean ? "플레이" : "Play")}",
                label);
            if (play)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting("legacyResult", false, $"결과창 {RDString.Get("status.results." + "early", null)}/{RDString.Get("status.results." + "tooEarly", null)} 위치 교환", $"Change {RDString.Get("status.results." + "early", null)}/{RDString.Get("status.results." + "tooEarly", null)} Position In Result", LegacyResult.OnLandOnPortalPatch.Patch);
                ShowSetting("noResult", false, "결과창 비활성화", "Disable Result");
                ShowSetting("hideDifficulty", false, "난이도 설정 비활성화", "Disable Difficulty Setting", c => {
                    if (c)
                        HideDifficulty();
                    else
                        ShowDifficulty();
                });
                ShowSetting("hideNoFail", false, "무적모드 비활성화", "Disable No Fail Mode", c => {
                    if (c)
                        HideNoFail();
                    else
                        ShowNoFail();
                });
                ShowSetting("oldPracticeMode", false, "P키로 연습모드", "Enable Practice Mode With P Key");
                ShowSetting("showSmallSpeedChange", false, "작은 속도변화 표시", "Show Small Speed Change");
                ShowSetting("noJudgeAnimation", false, "판정 확대 효과 없이 표시", "Show Judgement Without Animation");
                ShowSetting("lateJudgement", false, "판정 텍스트 한타일 앞에 띄우기", "Show Judgement Text On Prev Tile");
                ShowSetting("forceJudgeCount", false, "판정 텍스트 개수제한 (판정 당)", "Force Judgement Text Count (Per Judgement)");
                if (Settings.forceJudgeCount)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    ShowSlider("judgeCount", 1, 100);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            editor = GUILayout.Toggle(editor,
                $"{(editor ? "▼" : "▶")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "에디터" : "Editor")}",
                label);
            if (editor)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting("space360Tile", false, "스페이스바로 유턴 타일 생성", "Create 360 Tile With Space");
                ShowSetting("legacyTwirl", false, "옛날 소용돌이 사용", "Use Old Twirl", c =>
                {
                    if (scnEditor.instance != null)
                        scnEditor.instance.ApplyEventsToFloors();
                });
                ShowSetting("weakAuto", false, "오토 약화", "Use Weak Auto");
                ShowSetting("whiteAuto", false, "흰색 오토 고정", "Always Use White Auto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            sfx = GUILayout.Toggle(sfx,
                $"{(sfx ? "▼" : "▶")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "효과음" : "Sfx")}",
                label);
            if (sfx)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting("disablePurePerfectSound", false, "완벽한 플레이 소리 비활성화", "Disable Pure Perfect Sound");
                ShowSetting("disableWindSound", false, "화면 전환 시 바람소리 비활성화", "Disable Wind Sound When Wipe Screen");
                ShowSetting(null, typeof(GCS).GetField("playDeathSound"), true, "죽을 시 소리 비활성화", "Disable Death Sound");
                ShowSetting("disableCountdownSound", false, "카운트다운 소리 비활성화", "Disable Countdown Sound");
                ShowSetting("disableEndingSound", false, "클리어 소리 비활성화", "Disable Clear Sound");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            etc = GUILayout.Toggle(etc,
                $"{(etc ? "▼" : "▶")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "기타" : "Etc")}",
                label);
            if (etc)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting("legacyFont", false, "옛날 폰트 사용", "Use Old Font", c =>
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
                    ShowSetting("butNotJudgement", false, "판정 텍스트엔 적용하지 않기", "But Not For Judgement Text", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting("butNotCountdown", false, "카운트다운 텍스트엔 적용하지 않기", "But Not For Countdown Text", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting("butNotTitle", false, "맵 제목엔 적용하지 않기", "But Not For Level Title", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    ShowSetting("butNotSetting", false, "설정 텍스트엔 적용하지 않기", "But Not For Setting Text", c =>
                    {
                        RDString.initialized = false;
                        Persistence.Save();
                        ADOBase.RestartScene();
                    });
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
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

        private static void ShowSetting(string name, bool reverse, string korean, string english, Action<bool> onChange = null)
        {
            if (!typeof(Settings).Contains<bool>(name))
                throw new ArgumentException("no setting named " + name + "!");
            ShowSetting(Settings, typeof(Settings).GetField(name), reverse, korean, english, onChange);
        }

        private static void ShowSetting(object instance, FieldInfo field, bool reverse, string korean, string english, Action<bool> onChange = null)
        {
            bool prev = (bool)field.GetValue(instance);
            bool current = GUILayout.Toggle(prev,
                     $"{((reverse ? !prev : prev) ? "☑" : "☐")} " +
                     $"{(RDString.language == SystemLanguage.Korean ? korean : english)}",
                     label);
            field.SetValue(instance, current);
            if (prev != current)
                onChange?.Invoke(current);
        }

        private static void ShowSlider(string name, int min, int max, Action<int> onChange = null)
        {
            if (!typeof(Settings).Contains<int>(name))
                throw new ArgumentException("no slider setting named " + name + "!");
            ShowSlider(Settings, typeof(Settings).GetField(name), min, max, onChange);
        }

        private static void ShowSlider(object instance, FieldInfo field, int min, int max, Action<int> onChange = null)
        {
            int prev = (int)field.GetValue(instance);
            GUILayout.BeginHorizontal();
            int current = (int)GUILayout.HorizontalSlider(prev, min, max, GUILayout.Width(200));
            GUILayout.Label($"{current}", label);
            GUILayout.EndHorizontal();
            field.SetValue(instance, current);
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
