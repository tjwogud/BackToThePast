namespace BackToThePast.OldXO
{
    public static class OldXOTweak
    {
        public static void EnterLevel(bool speedTrial = false, bool changeScene = true)
        {
            GCS.sceneToLoad = "scnGame";
            GCS.customLevelPaths = new string[1] { "BackToThePast.OldXO" };
            GCS.customLevelIndex = 0;
            GCS.speedTrialMode = speedTrial;
            GCS.practiceMode = false;
            Persistence.SetSavedCurrentLevel("BackToThePast.OldXO." + (!speedTrial ? "default" : "speedTrial"));
            if (changeScene)
                scrController.instance.StartLoadingScene();
        }
    }
}
