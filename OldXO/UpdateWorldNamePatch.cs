using BackToThePast.Patch;
using System.Collections.Generic;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(PortalSign), "UpdateWorldName")]
    public static class UpdateWorldNamePatch
    {
		public static bool Prefix(PortalSign __instance, string world)
        {
            if (world == "BackToThePast.OldXO")
            {
                string text = RDString.Get("levelSelect.world", new Dictionary<string, object>
                {
                    {
                        "number",
                        "XO"
                    }
                }) + "?";
                __instance.worldName.text = text;
                return false;
            }
            return true;
        }
    }
}
