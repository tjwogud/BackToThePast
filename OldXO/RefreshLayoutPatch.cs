using HarmonyLib;
using System.Linq;

namespace BackToThePast.OldXO
{
    [HarmonyPatch(typeof(PauseMenu), "RefreshLayout")]
    public static class RefreshLayoutPatch
    {
        public static void Postfix(ref PauseButton[] ___pauseButtons, PauseButton ___openInEditorButton)
        {
            if (GCS.standaloneLevelMode && GCS.customLevelPaths != null && GCS.customLevelPaths.Length == 1 && GCS.customLevelPaths[0] == "BackToThePast.OldXO")
                ___pauseButtons = ___pauseButtons.Where(btn => btn != ___openInEditorButton).ToArray();
        }
    }
}
