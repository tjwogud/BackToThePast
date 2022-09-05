using BackToThePast.Patch;

namespace BackToThePast.LegacyCLS
{
    [BTTPPatch(typeof(scnCLS), "ToggleSearchMode")]
    public static class ToggleSearchModePatch
    {
        public static bool Prefix(bool search)
        {
            if (!Main.Settings.legacyCLS)
                return true;
            LegacyCLSTweak.ToggleSearchMode(search);
            return false;
        }
    }
}
