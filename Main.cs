using BackToThePast.HideAnnounceSign;
using BackToThePast.HideDifficulty;
using BackToThePast.HideNoFail;
using BackToThePast.LegacyCLS;
using BackToThePast.LegacyEditorButtons;
using BackToThePast.OldBackground;
using BackToThePast.Patch;
using BackToThePast.Utils;
using GDMiniJSON;
using HarmonyLib;
using Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace BackToThePast
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony Harmony;
        public static UnityModManager.ModEntry ModEntry;
        public static Settings Settings;
        public static Localization Localization;
        public static Font oldGodoMaum;
        public static FontData legacyFont;
        public static FontData originFont;
        public static Dictionary<string, object> oldXo;
        public static AudioClip oneForgottenNight;
        public static Dictionary<string, object> xoLevelMeta = new Dictionary<string, object>()
        {
            { "index", 1972 },
            { "levelCount", 1 },
            { "speedTrial", 1.1f },
            { "hasCheckpoints", false },
            { "floorPos", new Vector2Int(19, 26) }
        };
        public static bool lucky;
        public static readonly Type optionsPanelsCLS = typeof(ADOBase).Assembly.GetType("OptionsPanelsCLS");
        public static readonly Type option = typeof(ADOBase).Assembly.GetType("OptionsPanelsCLS+Option");
        public static readonly Type optionName = typeof(ADOBase).Assembly.GetType("OptionsPanelsCLS+OptionName");

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
            oldGodoMaum = bundle.LoadAsset<Font>("godoMaum");
            legacyFont = new FontData() {
                font = bundle.LoadAsset<Font>("Same_Mistake - Kopie"),
                fontScale = 0.95f,
                lineSpacing = 1.45f
            };
            oldXo = (Dictionary<string, object>)Json.Deserialize(bundle.LoadAsset<TextAsset>("old_xo").text);
            oneForgottenNight = bundle.LoadAsset<AudioClip>("One forgotten night");
            Logger.Log("Load Completed!");
            Logger.Log("Initializing Patches...");
            BTTPPatch.Init();
            Logger.Log("Completed!");
            Localization = Localization.Load("1QcrRL6LAs8WxJj_hFsEJa3CLM5g3e8Ya0KQlRKXwdlU", 343830105, modEntry);
            lucky = new System.Random().Next(20) == 0;

            object worldData;

            ConstructorInfo dictConst = AccessTools.Constructor(Reflections.GetType("GCNS+WorldData"), new Type[] { typeof(Dictionary<string, object>) });
            if (dictConst != null)
            {
                worldData = dictConst.Invoke(new object[] { xoLevelMeta });
            }
            else
            {
                worldData = AccessTools.GetDeclaredConstructors(Reflections.GetType("GCNS+WorldData"))[0].Invoke(new string[] { "index", "levelCount", "speedTrial", "hasCheckpoints", "floorPos", "floorPos" }.Select(s => xoLevelMeta[s]).ToArray());
            }
            object dict = typeof(GCNS).Get("worldData");
            AccessTools.Property(dict.GetType(), "Item").SetValue(dict, worldData, new object[] { "BackToThePast.OldXO" });
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                Harmony = new Harmony(modEntry.Info.Id);
                BTTPPatch.CheckSettings();
                RDString.initialized = false;
                SceneManager.activeSceneChanged += OnChangeScene;
            }
            else
            {
                Harmony.UnpatchAll(modEntry.Info.Id);
                SceneManager.activeSceneChanged -= OnChangeScene;
            }
            return true;
        }

        private static void OnChangeScene(Scene current, Scene next)
        {
            fontChanged = 0;
            OldBackgroundTweak.SetBackground();
            if (Settings.hideDifficulty)
                HideDifficultyTweak.ToggleDifficulty(false);
            if (Settings.hideNoFail)
                HideNoFailTweak.ToggleNoFail(false);
        }

        private static bool initialized = false;

        private static GUIStyle label;
        private static GUIStyle smallLabel;
        private static GUIStyle disabledLabel;
        private static GUIStyle btn;
        private static bool play = false;
        private static bool editor = false;
        private static bool sfx = false;
        private static bool font = false;
        private static bool etc = false;

        private static bool changed;
        private static int fontChanged = 0;

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
                smallLabel = new GUIStyle(GUI.skin.label);
                smallLabel.fontSize = 16;
                disabledLabel = new GUIStyle(label);
                disabledLabel.normal.textColor = Color.red;
                btn = new GUIStyle(GUI.skin.button);
                btn.fontSize = 16;
            }

            changed = false;

            play = GUILayout.Toggle(play,
                $"{(play ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.play"]}",
                label);
            if (play)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                ShowSetting(Settings.legacyResult, b => Settings.legacyResult = b, "legacyResult");
                ShowSetting(Settings.noResult, b => Settings.noResult = b, "noResult");
                if (scrController.instance != null && !scrController.instance.paused && scrController.instance.gameworld)
                {
                    ShowDummySetting(Settings.hideDifficulty, "hideDifficulty");
                    ShowDummySetting(Settings.hideNoFail, "hideNoFail");
                } else
                {
                    ShowSetting(Settings.hideDifficulty, b => Settings.hideDifficulty = b, "hideDifficulty", c => HideDifficultyTweak.ToggleDifficulty(!c));
                    ShowSetting(Settings.hideNoFail, b => Settings.hideNoFail = b, "hideNoFail", c => HideNoFailTweak.ToggleNoFail(!c));
                }
                ShowSetting(Settings.oldPracticeMode, b => Settings.oldPracticeMode = b, "oldPracticeMode");
                ShowSetting(Settings.showSmallSpeedChange, b => Settings.showSmallSpeedChange = b, "showSmallSpeedChange", c => scnEditor.instance?.ApplyEventsToFloors());
                if (Settings.showSmallSpeedChange)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowSetting(Settings.showDetailSpeedChange, b => Settings.showDetailSpeedChange = b, "showDetailSpeedChange", c => scnEditor.instance?.ApplyEventsToFloors());
                    GUILayout.EndHorizontal();
                    if (Settings.showDetailSpeedChange)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        GUILayout.BeginVertical();
                        GUILayout.Label(Localization["bttp.settings.minBpmToShowSpeedChange"], smallLabel);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(Localization["bttp.settings.minBpmToShowSpeedChange.expression"], smallLabel);
                        string current = GUILayout.TextField(string.Format("{0:0.0000}", Settings.minBpmToShowSpeedChange), GUILayout.Width(50), GUILayout.Height(23));
                        if (float.TryParse(current, out float result) && result >= 0 && result != Settings.minBpmToShowSpeedChange)
                        {
                            Settings.minBpmToShowSpeedChange = result;
                            scnEditor.instance?.ApplyEventsToFloors();
                        }
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }
                }
                ShowSetting(Settings.legacyFlash, b => Settings.legacyFlash = b, "legacyFlash");
                ShowSetting(Settings.noJudgeAnimation, b => Settings.noJudgeAnimation = b, "noJudgeAnimation");
                ShowSetting(Settings.lateJudgement, b => Settings.lateJudgement = b, "lateJudgement");
                ShowSetting(Settings.forceJudgeCount, b => Settings.forceJudgeCount = b, "forceJudgeCount");
                if (Settings.forceJudgeCount)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowIntSlider(Settings.judgeCount, i => Settings.judgeCount = i, 1, 100);
                    GUILayout.EndHorizontal();
                }
                ShowSetting(Settings.legacyTwirl, b => Settings.legacyTwirl = b, "legacyTwirl", c => scnEditor.instance?.ApplyEventsToFloors());
                if (Settings.legacyTwirl)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowSetting(Settings.twirlWithoutArrow, b => Settings.twirlWithoutArrow = b, "twirlWithoutArrow", c => scnEditor.instance?.ApplyEventsToFloors());
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
                ShowSetting(Settings.space360Tile, b => Settings.space360Tile = b, "space360Tile");
                ShowSetting(Settings.weakAuto, b => Settings.weakAuto = b, "weakAuto", c => RDC.useOldAuto = c);
                ShowSetting(Settings.whiteAuto, b => Settings.whiteAuto = b, "whiteAuto");
                ShowSetting(Settings.legacyEditorButtonsPositions, b => Settings.legacyEditorButtonsPositions = b, "legacyEditorButtonsPositions", LegacyEditorButtonsTweak.ChangeEditorButtons);
                ShowSetting(Settings.legacyEditorButtonsDesigns, b => Settings.legacyEditorButtonsDesigns = b, "legacyEditorButtonsDesigns", LegacyEditorButtonsTweak.RemoveShadowAddOutline);
                if (RDString.language == SystemLanguage.Korean)
                    ShowSetting(Settings.legacyTexts, b => Settings.legacyTexts = b, "legacyTexts", c =>
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
                ShowSetting(Settings.disablePurePerfectSound, b => Settings.disablePurePerfectSound = b, "disablePurePerfectSound");
                ShowSetting(Settings.disableWindSound, b => Settings.disableWindSound = b, "disableWindSound");
                ShowSetting(!GCS.playDeathSound, b => GCS.playDeathSound = !b, "disableDeathSound");
                ShowSetting(Settings.disableCountdownSound, b => Settings.disableCountdownSound = b, "disableCountdownSound");
                ShowSetting(Settings.disableEndingSound, b => Settings.disableEndingSound = b, "disableEndingSound");
                ShowSetting(Settings.disableNewBestSound, b => Settings.disableNewBestSound = b, "disableNewBestSound");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            font = GUILayout.Toggle(font,
                $"{(font ? "▼" : "▶")} " +
                $"{Localization["bttp.settings.font"]}",
                label);
            if (font)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                GUILayout.Label(Localization["bttp.settings.font.description"], label);

                ShowSetting(Settings.legacyFont, b => Settings.legacyFont = b, "legacyFont", c =>
                {
                    if (c)
                        fontChanged += 1;
                    else
                        fontChanged -= 1;
                });
                if (Settings.legacyFont)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    ShowSetting(Settings.butNotJudgement, b => Settings.butNotJudgement = b, "butNotJudgement", c =>
                    {
                        if (c)
                            fontChanged += 2;
                        else
                            fontChanged -= 2;
                    });
                    ShowSetting(Settings.butNotCountdown, b => Settings.butNotCountdown = b, "butNotCountdown", c =>
                    {
                        if (c)
                            fontChanged += 4;
                        else
                            fontChanged -= 4;
                    });
                    ShowSetting(Settings.butNotTitle, b => Settings.butNotTitle = b, "butNotTitle", c =>
                    {
                        if (c)
                            fontChanged += 8;
                        else
                            fontChanged -= 8;
                    });
                    ShowSetting(Settings.butNotSetting, b => Settings.butNotSetting = b, "butNotSetting", c =>
                    {
                        if (c)
                            fontChanged += 16;
                        else
                            fontChanged -= 16;
                    });
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                ShowSetting(Settings.oldGodoMaum, b => Settings.oldGodoMaum = b, "oldGodoMaum", c =>
                {
                    if (c)
                        fontChanged += 32;
                    else
                        fontChanged -= 32;
                });
                if (fontChanged != 0)
                {
                    if (GUILayout.Button(Localization["bttp.settings.font.refresh"], GUILayout.Width(200)))
                    {
                        fontChanged = 0;
                        RDString.initialized = false;
                        RDString.Setup();
                        Persistence.Save();
                        ADOBase.RestartScene();
                    }
                }
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
                if (optionsPanelsCLS != null)
                    ShowSetting(Settings.legacyCLS, b => Settings.legacyCLS = b, "legacyCLS", LegacyCLSTweak.Toggle);
                ShowSetting(Settings.disableAlphaWarning, b => Settings.disableAlphaWarning = b, "disableAlphaWarning");
                ShowSetting(Settings.disableAnnounceSign, b => Settings.disableAnnounceSign = b, "disableAnnounceSign", c => HideAnnounceSignTweak.ToggleSign(!c));
                ShowSetting(Settings.oldBackground, b => Settings.oldBackground = b, "oldBackground", c => OldBackgroundTweak.SetBackground());
                if (Settings.oldBackground)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    ShowToggleGroup(Settings.oldBackgroundIndex, i => Settings.oldBackgroundIndex = i, new string[] { "A", "B" }, c => OldBackgroundTweak.SetBackground());
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            if (changed)
                BTTPPatch.CheckSettings();

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

        private static void ShowDummySetting(bool prev, string key)
        {
            GUILayout.Label($"{(prev ? "☑" : "☐")} {Localization[$"bttp.settings.{key}"]}", disabledLabel);
        }

        private static void ShowSetting(bool prev, Action<bool> setter, string key, Action<bool> onChange = null)
        {
            bool current = GUILayout.Toggle(prev, $"{(prev ? "☑" : "☐")} {Localization[$"bttp.settings.{key}"]}", label);
            setter.Invoke(current);
            if (prev != current)
            {
                onChange?.Invoke(current);
                changed = true;
            }
        }

        private static void ShowIntSlider(int prev, Action<int> setter, int min, int max, Action<int> onChange = null)
        {
            ShowSlider(prev, f => setter.Invoke((int)f), min, max, 0, f => onChange.Invoke((int)f));
        }

        private static void ShowSlider(float prev, Action<float> setter, float min, float max, int decimals = -1, Action<float> onChange = null)
        {
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

        private static void ShowToggleGroup(int prev, Action<int> setter, string[] arr, Action<int> onChange = null)
        {
            GUILayout.BeginHorizontal();
            int current = prev;
            for (int i = 0; i < arr.Length; i++)
            {
                if (GUILayout.Button(prev == i ? $"<b>{arr[i]}</b>" : arr[i], GUILayout.Width(100)))
                {
                    current = i;
                }
            }
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
