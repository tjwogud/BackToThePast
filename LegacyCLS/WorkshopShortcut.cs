using BackToThePast.Utils;
using System;
using UnityEngine;

namespace BackToThePast.LegacyCLS
{
    public class WorkshopShortcut : ADOBase
    {
        private void Update()
        {
            if (Main.Settings.legacyCLS && !controller.paused && !LegacyCLSTweak.searchMode && controller.responsive && !scnCLS.instance.showingInitialMenu)
            {
                if (Input.GetKeyDown(KeyCode.F))
                    LegacyCLSTweak.ToggleSearchMode(true);
                else if (Input.GetKeyDown(KeyCode.S))
                    scnCLS.instance.Get("optionsPanels").Method("ToggleSpeedTrial");
                else if (Input.GetKeyDown(KeyCode.N))
                    scnCLS.instance.Get("optionsPanels").Method("ToggleNoFail");
                else if (Input.GetKeyDown(KeyCode.Delete))
                    scnCLS.instance.DeleteLevel();
                else if (Input.GetKeyDown(KeyCode.O))
                {
                    object optionsPanels = scnCLS.instance.Get("optionsPanels");
                    object sortings = optionsPanels.Get("sortings");
                    int num = Array.IndexOf(sortings as Array, optionsPanels.Get("sortingMethod"));
                    num = (num == sortings.Get<int>("Length") - 1) ? 0 : (num + 1);
                    object sortingMethod = (sortings as Array).GetValue(num);
                    optionsPanels.Set("sortingMethod", sortingMethod);
                    optionsPanels.Method("SelectOption", new object[] { sortingMethod, true }, new Type[] { Main.optionName, typeof(bool) });
                    optionsPanels.Method("UpdateSorting");
                }
            }
        }
    }
}
