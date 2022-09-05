using BackToThePast.Patch;
using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace BackToThePast
{
    public class Settings : UnityModManager.ModSettings
    {
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = Path.Combine(modEntry.Path, "Settings.xml");
            using (var writer = new StreamWriter(filepath))
                new XmlSerializer(GetType()).Serialize(writer, this);
        }

        [TogglePatch("OldXO")]
        public bool oldXO = true;

        [TogglePatch("LegacyResult")]
        public bool legacyResult = false;
        [TogglePatch("NoResult")]
        public bool noResult = false;
        [TogglePatch("HideDifficulty")]
        public bool hideDifficulty = false;
        [TogglePatch("HideNoFail")]
        public bool hideNoFail = false;
        [TogglePatch("OldPracticeMode")]
        public bool oldPracticeMode = false;
        [TogglePatch("ShowSmallSpeedChange")]
        public bool showSmallSpeedChange = false;
        [TogglePatch("LegacyFlash")]
        public bool legacyFlash = false;
        [TogglePatch("JudgementText")]
        public bool noJudgeAnimation = false;
        [TogglePatch("JudgementText")]
        public bool lateJudgement = false;
        [TogglePatch("JudgementText")]
        public bool forceJudgeCount = false;
            public int judgeCount = 100;
        [TogglePatch("LegacyTwirl")]
        public bool legacyTwirl = false;
            public bool twirlWithoutArrow = false;

        [TogglePatch("Space360Tile")]
        public bool space360Tile = false;
        [TogglePatch("WeakAuto")]
        public bool weakAuto = false;
        [TogglePatch("WhiteAuto")]
        public bool whiteAuto = false;
        [TogglePatch("LegacyEditorButtons")]
        public bool legacyEditorButtonsPositions = false;
        [TogglePatch("LegacyEditorButtons")]
        public bool legacyEditorButtonsDesigns = false;
        [TogglePatch("LegacyName")]
        public bool legacyTexts = false;

        [TogglePatch("NoSfxSound")]
        public bool disablePurePerfectSound = false;
        [TogglePatch("NoSfxSound")]
        public bool disableWindSound = false;
        [TogglePatch("NoSfxSound")]
        public bool disableCountdownSound = false;
        [TogglePatch("NoSfxSound")]
        public bool disableEndingSound = false;

        [TogglePatch("LegacyFont")]
        public bool legacyFont = false;
            public bool butNotJudgement = false;
            public bool butNotCountdown = false;
            public bool butNotTitle = false;
            public bool butNotSetting = false;
        [TogglePatch("LegacyCLS", "OptionsPanelsCLS")]
        public bool legacyCLS = false;
        [TogglePatch("HideAlphaWarning")]
        public bool disableAlphaWarning = false;
    }
}
