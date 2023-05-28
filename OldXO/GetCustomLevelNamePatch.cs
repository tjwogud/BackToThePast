using BackToThePast.Patch;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(ADOBase), "GetCustomLevelName")]
    public static class GetCustomLevelNamePatch
    {
        public static bool Prefix(string path, ref string __result)
        {
            if (path == "BackToThePast.OldXO")
            {
                __result = scnGame.instance.levelData.fullCaption;
                return false;
            }
            return true;
        }
    }
}
