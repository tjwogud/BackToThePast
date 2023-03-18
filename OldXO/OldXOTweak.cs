namespace BackToThePast.OldXO
{
    public static class OldXOTweak
    {
        public static void EnterLevel(bool speedTrial = false, bool changeScene = true)
        {
            GCS.sceneToLoad = "scnEditor";
            GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
            GCS.speedTrialMode = speedTrial;
            GCS.practiceMode = false;
            GCS.standaloneLevelMode = true;
            Persistence.SetSavedCurrentLevel("BackToThePast.OldXO." + (!speedTrial ? "default" : "speedTrial"));
            if (changeScene)
                scrController.instance.StartLoadingScene();
        }
    }
}
