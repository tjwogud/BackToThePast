using HarmonyLib;
using System;
using System.Runtime.CompilerServices;

namespace BackToThePast.Patch
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class BTTPPatchAttribute : HarmonyPatch
	{
		public BTTPPatchAttribute() : base() { }

		public BTTPPatchAttribute(Type declaringType, string methodName, Type[] argumentTypes = null, [CallerMemberName] string typeName = null) : base(declaringType, methodName, argumentTypes)
		{
			if ((argumentTypes != null ? AccessTools.Method(declaringType, methodName, argumentTypes) : AccessTools.Method(declaringType, methodName)) != null)
				return;
			Main.Logger.Log($"No method! ({typeName}.cs)");
		}

		[AttributeUsage(AttributeTargets.Class)]
		public class IgnoreSettingAttribute : Attribute
		{
		}
	}
}
