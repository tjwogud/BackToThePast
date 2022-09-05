using BackToThePast.Patch;

namespace BackToThePast.WeakAuto
{
    [BTTPPatch(typeof(scrShowIfDebug), "Update")]
    public static class UpdatePatch
    {
        public static void Prefix()
        {
            RDC.useOldAuto = false;
        }

        public static void Postfix()
        {
            RDC.useOldAuto = Main.Settings.weakAuto;
        }
    }
}
