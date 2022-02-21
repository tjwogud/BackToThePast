using BackToThePast.Utils;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
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
        public static Font legacyFont;

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
            legacyFont = bundle.LoadAsset<Font>("Same_Mistake - Kopie");
            Logger.Log("Load Completed!");
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static bool initialized = false;

        private static GUIStyle label;
        private static bool play = false;
        private static bool editor = false;
        private static bool sfx = false;
        private static bool etc = false;

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (!initialized)
            {
                initialized = true;
                label = new GUIStyle(GUI.skin.label);
                label.fontSize = 18;
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
                Settings.noPracticeMode = GUILayout.Toggle(Settings.noPracticeMode,
                    $"{(Settings.noPracticeMode ? "☑" : "☐")} " +
                    $"{(RDString.language == SystemLanguage.Korean ? "연습모드 비활성화" : "Disable Practice Mode")}",
                    label);
                GUILayout.EndVertical();
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
                Settings.space360Tile = GUILayout.Toggle(Settings.space360Tile,
                    $"{(Settings.space360Tile ? "☑" : "☐")} " +
                    $"{(RDString.language == SystemLanguage.Korean ? "스페이스바로 유턴 타일 생성" : "Create 360 Tile With Space")}",
                    label);
                var prev = Settings.legacyTwirl;
                Settings.legacyTwirl = GUILayout.Toggle(Settings.legacyTwirl,
                    $"{(Settings.legacyTwirl ? "☑" : "☐")} " +
                    $"{(RDString.language == SystemLanguage.Korean ? "옛날 소용돌이 사용" : "Use Old Twirl")}",
                    label);
                if (prev != Settings.legacyTwirl && scnEditor.instance != null)
                    scnEditor.instance.ApplyEventsToFloors();
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
                Settings.disablePurePerfectSound = GUILayout.Toggle(Settings.disablePurePerfectSound,
                   $"{(Settings.disablePurePerfectSound ? "☑" : "☐")} " +
                   $"{(RDString.language == SystemLanguage.Korean ? "완벽한 플레이 소리 비활성화" : "Disable Pure Perfect Sound")}",
                   label);
                Settings.disableWindSound = GUILayout.Toggle(Settings.disableWindSound,
                   $"{(Settings.disableWindSound ? "☑" : "☐")} " +
                   $"{(RDString.language == SystemLanguage.Korean ? "화면 전환 시 바람소리 비활성화" : "Disable Wind Sound When Wipe Screen")}",
                   label);
                GCS.playDeathSound = GUILayout.Toggle(GCS.playDeathSound,
                   $"{(!GCS.playDeathSound ? "☑" : "☐")} " +
                   $"{(RDString.language == SystemLanguage.Korean ? "죽을 시 소리 비활성화" : "Disable Death Sound")}",
                   label);
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
                var prev = Settings.legacyFont;
                Settings.legacyFont = GUILayout.Toggle(Settings.legacyFont,
                   $"{(Settings.legacyFont ? "☑" : "☐")} " +
                   $"{(RDString.language == SystemLanguage.Korean ? "예전 폰트 사용" : "Use Old Font")}",
                   label);
                if (prev != legacyFont)
                    RDString.ChangeLanguage(RDString.language);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Settings...");
            Settings.Save(modEntry);
            Logger.Log("Save Completed!");
        }
    }
}
