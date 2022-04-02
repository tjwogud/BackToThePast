
using HarmonyLib;
using System.Text;

namespace BackToThePast.LegacyResult
{
    [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
    public static class OnLandOnPortalPatch
    {
        public static void Postfix(scrController __instance)
        {
            if (__instance.gameworld && Main.Settings.legacyResult && __instance.txtResults.gameObject.activeSelf)
            {
				int resultCount = __instance.mistakesManager.GetHits(HitMargin.Perfect);
				int resultCount2 = __instance.mistakesManager.GetHits(HitMargin.EarlyPerfect);
				int resultCount3 = __instance.mistakesManager.GetHits(HitMargin.LatePerfect);
				int resultCount4 = __instance.mistakesManager.GetHits(HitMargin.VeryEarly);
				int resultCount5 = __instance.mistakesManager.GetHits(HitMargin.VeryLate);
				int resultCount6 = __instance.mistakesManager.GetHits(HitMargin.TooEarly);
				int resultCount7 = __instance.mistakesManager.GetHits(HitMargin.FailMiss);
				int resultCount8 = __instance.mistakesManager.GetHits(HitMargin.FailOverload);
				float num2 = Persistence.GetShowXAccuracy() ? __instance.mistakesManager.percentXAcc : __instance.mistakesManager.percentAcc;
				ColourSchemeHitMargin hitMarginColoursUI = RDConstants.data.hitMarginColoursUI;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Localized("ePerfect")).Append(Result(resultCount2, hitMarginColoursUI.colourLittleEarly.ToHex())).Append("     ");
				stringBuilder.Append(Localized("perfect")).Append(Result(resultCount, hitMarginColoursUI.colourPerfect.ToHex())).Append("     ");
				stringBuilder.Append(Localized("lPerfect")).Append(Result(resultCount3, hitMarginColoursUI.colourLittleLate.ToHex())).Append("\n");
				stringBuilder.Append(Localized("early")).Append(Result(resultCount4, hitMarginColoursUI.colourVeryEarly.ToHex())).Append("     ");
				stringBuilder.Append(Localized("tooEarly")).Append(Result(resultCount6, hitMarginColoursUI.colourTooEarly.ToHex())).Append("     ");
				stringBuilder.Append(Localized("late")).Append(Result(resultCount5, hitMarginColoursUI.colourVeryLate.ToHex())).Append("\n");
				if (__instance.noFail)
				{
					stringBuilder.Append(Localized("missFails")).Append(Result(resultCount7, hitMarginColoursUI.colourFail.ToHex())).Append("     ");
					stringBuilder.Append(Localized("overloadFails")).Append(Result(resultCount8, hitMarginColoursUI.colourFail.ToHex())).Append("\n");
				}
				stringBuilder.Append(Localized(Persistence.GetShowXAccuracy() ? "xAccuracy" : "accuracy")).Append(GoldAccuracy(string.Format("{0:0.00}%", num2 * 100f), __instance.mistakesManager.IsAllPurePerfect())).Append("     ");
				stringBuilder.Append(Localized("checkpoints")).Append(scrController.checkpointsUsed.ToString()).Append("\n");
				__instance.txtResults.text = stringBuilder.ToString();
			}
        }

		private static string Localized(string s)
		{
			return RDString.Get("status.results." + s, null) + ": ";
		}

		private static string Result(int resultCount, string color)
		{
			return string.Format("<color={0}>{1}</color>", color, resultCount);
		}
		private static string GoldAccuracy(string accText, bool isPurePerfect)
		{
			if (!isPurePerfect)
				return accText;
			return "<color=#FFDA00>" + accText + "</color>";
		}
	}
}
