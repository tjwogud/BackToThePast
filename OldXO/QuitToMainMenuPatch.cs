﻿using BackToThePast.Patch;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrController), "QuitToMainMenu")]
    public static class QuitToMainMenuPatch
    {
        public static void Prefix()
        {
            if (ADOBase.isScnGame && GCS.customLevelPaths != null && GCS.customLevelPaths.Length == 1 && GCS.customLevelPaths[0] == "BackToThePast.OldXO")
            {
                GCS.customLevelPaths = null;
                GCS.worldEntrance = "BackToThePast.SecretIsland";
            }
        }
    }
}
