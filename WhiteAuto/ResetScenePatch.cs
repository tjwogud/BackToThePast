using HarmonyLib;

namespace BackToThePast.WhiteAuto
{
    [HarmonyPatch(typeof(CustomLevel), "ResetScene")]
    public static class ResetScenePatch
    {
        public static void Postfix()
        {
            if (scnEditor.instance != null)
                scnEditor.instance.autoFailed = false;
        }
    }
}
