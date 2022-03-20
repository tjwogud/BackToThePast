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

        public bool legacyResult = false;
        public bool noResult = false;
        public bool noPracticeMode = false;
        public bool hideDifficulty = false;
        public bool hideNoFail = false;
        public bool showSmallSpeedChange = false;
        public bool lateJudgement = false;
        public bool forceJudgeCount = false;
        public int judgeCount = 100;

        public bool space360Tile = false;
        public bool legacyTwirl = false;
        public bool weakAuto = false;
        public bool whiteAuto = false;

        public bool disablePurePerfectSound = false;
        //deathsound is already on game
        public bool disableWindSound = false;
        public bool disableCountdownSound = false;
        public bool disableEndingSound = false;

        public bool legacyFont = false;
        public bool butNotJudgement = false;
    }
}
