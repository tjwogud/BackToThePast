using BackToThePast.Utils;
using HarmonyLib;
using System.Reflection;
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

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (!initialized)
            {
                initialized = true;
                label = new GUIStyle(GUI.skin.label);
                label.fontSize = 18;
            }

            var pauseButtons = GUILayout.Toggle(Settings.pauseButtons,
                $"{(Settings.pauseButtons ? "☑" : "☐")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "수정/나가기 버튼 위치 교체" : "Switch Edit/Quit Button")}",
                label);
            if (pauseButtons != Settings.pauseButtons)
            {
                Settings.pauseButtons = pauseButtons;
                if (scrController.instance.paused)
                    scrController.instance.pauseMenu.Method("RefreshLayout");
            }

            Settings.space360Tile = GUILayout.Toggle(Settings.space360Tile,
                $"{(Settings.space360Tile ? "☑" : "☐")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "스페이스바로 유턴 타일 생성" : "Create 360 Tile With Space")}",
                label);

            Settings.noPracticeMode = GUILayout.Toggle(Settings.noPracticeMode,
                $"{(Settings.noPracticeMode ? "☑" : "☐")} " +
                $"{(RDString.language == SystemLanguage.Korean ? "연습모드 비활성화" : "Disable Practice Mode")}",
                label);
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Settings...");
            Settings.Save(modEntry);
            Logger.Log("Save Completed!");
        }
    }
}
