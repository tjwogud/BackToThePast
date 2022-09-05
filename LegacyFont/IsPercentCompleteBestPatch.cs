using BackToThePast.Patch;

namespace BackToThePast.LegacyFont
{
    [BTTPPatch(typeof(scrController), "IsPercentCompleteBest")]
    public static class IsPercentCompleteBestPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (scnEditor.instance != null)
                __result = false;
        }
    }
}
