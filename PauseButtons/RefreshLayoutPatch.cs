using BackToThePast.Utils;
using HarmonyLib;

namespace BackToThePast.PauseButtons
{
    [HarmonyPatch(typeof(PauseMenu), "RefreshLayout")]
    public static class RefreshLayoutPatch
    {
        public static void Postfix(PauseMenu __instance)
        {
            if (!Main.Settings.pauseButtons)
                return;
            var buttons = __instance.Get<GeneralPauseButton[]>("pauseButtons");
            if (buttons != null && (buttons[buttons.Length - 1] as PauseButton)?.rdString == __instance.openInEditorButton.rdString)
            {
                var button = buttons[buttons.Length - 1];
                buttons[buttons.Length - 1] = buttons[buttons.Length - 2];
                buttons[buttons.Length - 2] = button;
            }
        }
    }
}
