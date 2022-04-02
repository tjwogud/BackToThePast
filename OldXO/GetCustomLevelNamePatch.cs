using HarmonyLib;

namespace BackToThePast.OldXO
{
    [HarmonyPatch(typeof(ADOBase), "GetCustomLevelName")]
    public static class GetCustomLevelNamePatch
    {
        public static bool Prefix(string path, ref string __result)
        {
            if (path == "BackToThePast.OldXO")
            {
                __result = CustomLevel.instance.levelData.fullCaption;
                return false;
            }
            return true;
        }
    }
}
