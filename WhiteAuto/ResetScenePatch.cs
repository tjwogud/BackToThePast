using BackToThePast.Patch;

namespace BackToThePast.WhiteAuto
{
    [BTTPPatch(typeof(CustomLevel), "ResetScene")]
    public static class ResetScenePatch
    {
        public static void Postfix()
        {
            if (scnEditor.instance != null)
                scnEditor.instance.autoFailed = false;
        }
    }
}
