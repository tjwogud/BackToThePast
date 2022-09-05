using BackToThePast.Patch;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BackToThePast.LegacyResult
{
	[BTTPPatch(typeof(scrController), "OnLandOnPortal")]
    public static class OnLandOnPortalPatch
    {
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
			List<CodeInstruction> tooEarlyInstructions = new List<CodeInstruction>();
			bool tooEarly = false;
			bool afterTooEarly = false;
			for (int i = 0; i < instructions.Count(); i++)
            {
				CodeInstruction instruction = instructions.ElementAt(i);
				if (i < instructions.Count() - 1 && instructions.ElementAt(i + 1).operand as string == "tooEarly")
					tooEarly = true;
				if (tooEarly)
                {
					tooEarlyInstructions.Add(instruction);
					if (instruction.operand == null)
					{
						tooEarly = false;
						afterTooEarly = true;
					}
                } else
                {
					yield return instruction;
					if (afterTooEarly && instruction.operand == null)
                    {
						afterTooEarly = false;
						foreach (CodeInstruction instruction1 in tooEarlyInstructions)
							yield return instruction1;
                    }
                }
			}
        }
	}
}
