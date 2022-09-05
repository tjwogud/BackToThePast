using BackToThePast.Patch;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrController), "PortalTravelAction")]
    public static class PortalTravelActionPatch
    {
        public static void Postfix(int ___portalDestination)
        {
            if (___portalDestination != -2)
                return;
            string current = Persistence.GetSavedCurrentLevel();
            if (current == "BackToThePast.OldXO.default")
            {
                GCS.sceneToLoad = "scnEditor";
                GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
                GCS.speedTrialMode = false;
                GCS.practiceMode = false;
                GCS.standaloneLevelMode = true;
            }
            else if (current == "BackToThePast.OldXO.speedTrial")
            {
                GCS.sceneToLoad = "scnEditor";
                GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
                GCS.speedTrialMode = true;
                GCS.practiceMode = false;
                GCS.standaloneLevelMode = true;
            }
        }
    }
}
