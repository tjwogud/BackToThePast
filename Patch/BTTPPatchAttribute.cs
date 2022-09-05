using HarmonyLib;
using System;

namespace BackToThePast.Patch
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class BTTPPatchAttribute : HarmonyPatch
	{
		public BTTPPatchAttribute() { }
		public BTTPPatchAttribute(Type declaringType) : base(declaringType) { }
		public BTTPPatchAttribute(Type declaringType, Type[] argumentTypes) : base(declaringType, argumentTypes) { }
		public BTTPPatchAttribute(Type declaringType, string methodName) : base(declaringType, methodName) { }
		public BTTPPatchAttribute(Type declaringType, string methodName, params Type[] argumentTypes) : base(declaringType, methodName, argumentTypes) { }
		public BTTPPatchAttribute(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations) : base(declaringType, methodName, argumentTypes, argumentVariations) { }
		public BTTPPatchAttribute(Type declaringType, MethodType methodType) : base(declaringType, methodType) { }
		public BTTPPatchAttribute(Type declaringType, MethodType methodType, params Type[] argumentTypes) : base(declaringType, methodType, argumentTypes) { }
		public BTTPPatchAttribute(Type declaringType, MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations) : base(declaringType, methodType, argumentTypes, argumentVariations) { }
		public BTTPPatchAttribute(Type declaringType, string methodName, MethodType methodType) : base(declaringType, methodName, methodType) { }
		public BTTPPatchAttribute(string methodName) : base(methodName) { }
		public BTTPPatchAttribute(string methodName, params Type[] argumentTypes) : base(methodName, argumentTypes) { }
		public BTTPPatchAttribute(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations) : base(methodName, argumentTypes, argumentVariations) { }
		public BTTPPatchAttribute(string methodName, MethodType methodType) : base(methodName, methodType) { }
		public BTTPPatchAttribute(MethodType methodType) : base(methodType) { }
		public BTTPPatchAttribute(MethodType methodType, params Type[] argumentTypes) : base(methodType, argumentTypes) { }
		public BTTPPatchAttribute(MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations) : base(methodType, argumentTypes, argumentVariations) { }
		public BTTPPatchAttribute(Type[] argumentTypes) : base(argumentTypes) { }
		public BTTPPatchAttribute(Type[] argumentTypes, ArgumentType[] argumentVariations) : base(argumentTypes, argumentVariations) { }

		[AttributeUsage(AttributeTargets.Class)]
		public class IgnoreSettingAttribute : Attribute
		{
		}
	}
}
