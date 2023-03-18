using BackToThePast.Patch;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(scrController), "PortalTravelAction")]
    public static class PortalTravelActionPatch
    {
        public static bool Prefix(scrController __instance, int ___portalDestination)
        {
            if (___portalDestination != -2)
                return true;
            string current = Persistence.GetSavedCurrentLevel();
            if (current == "BackToThePast.OldXO.default")
            {
                OldXOTweak.EnterLevel();
                return false;
            }
            else if (current == "BackToThePast.OldXO.speedTrial")
            {
                OldXOTweak.EnterLevel(true);
                return false;
            }
            return true;
        }
    }
}
