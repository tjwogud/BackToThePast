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

        public bool noPracticeMode = false;

        public bool space360Tile = false;
        public bool legacyTwirl = false;

        public bool disablePurePerfectSound = false;
        //deathsound is already on game
        public bool disableWindSound = false;

        public bool legacyFont = false;
    }
}
