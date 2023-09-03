using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static BackToThePast.Patch.BTTPPatchAttribute;

namespace BackToThePast.Patch
{
	public static class BTTPPatch
	{
		private static List<(Type, bool)> ignoreSettingPatches = new List<(Type, bool)>();
		private static readonly Dictionary<string, (List<Type>, bool)> patches = new Dictionary<string, (List<Type>, bool)>();
		private static readonly Dictionary<string, List<FieldInfo>> settings = new Dictionary<string, List<FieldInfo>>();
		private static bool initialized = false;

		public static void Init()
		{
			if (initialized)
				return;
            Assembly.GetExecutingAssembly().GetTypes().Where(type =>
			{
				if (type.GetCustomAttributes<BTTPPatchAttribute>().Count() == 0)
					return false;
				if (type.GetCustomAttributes<IgnoreSettingAttribute>().Count() == 0)
					return true;
				ignoreSettingPatches.Add((type, false));
				return false;
			}).ToList().ForEach(type =>
			{
				string id = type.Namespace.Replace("BackToThePast.", "");
				(List<Type>, bool) tuple = patches.TryGetValue(id, out (List<Type>, bool) value) ? value : (new List<Type>(), false);
				tuple.Item1.Add(type);
				patches[id] = tuple;
			});
			typeof(Settings).GetFields(AccessTools.all).ToList().ForEach(field =>
			{
				TogglePatchAttribute attribute = field.GetCustomAttribute<TogglePatchAttribute>();
				if (attribute == null || attribute.Disabled)
					return;
				string id = attribute.Id;
				List<FieldInfo> list = settings.TryGetValue(id, out List<FieldInfo> value) ? value : new List<FieldInfo>();
				list.Add(field);
				settings[id] = list;
			});
			initialized = true;
        }

		public static void CheckSettings()
        {
			ignoreSettingPatches = ignoreSettingPatches.Select(tuple =>
			{
				if (tuple.Item2)
					return tuple;
				Main.Harmony.CreateClassProcessor(tuple.Item1).Patch();
				return (tuple.Item1, true);
			}).ToList();
			settings.ToList().ForEach(pair =>
			{
				if (pair.Value.Any(field => (bool)field.GetValue(Main.Settings)))
					Patch(pair.Key);
				else
					UnPatch(pair.Key);
			});
        }

        public static bool Patch(string id)
        {
            if (!patches.TryGetValue(id, out (List<Type>, bool) pair) || pair.Item2)
                return false;
            pair.Item1.ForEach(type => Main.Harmony.CreateClassProcessor(type).Patch());
			patches[id] = (pair.Item1, true);
            return true;
        }

        public static bool UnPatch(string id)
		{
			if (!patches.TryGetValue(id, out (List<Type>, bool) pair) || !pair.Item2)
				return false;
			using (List<MethodBase>.Enumerator enumerator = Main.Harmony.GetPatchedMethods().ToList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MethodBase original = enumerator.Current;
					bool flag = original.HasMethodBody();
					Patches patchInfo2 = Harmony.GetPatchInfo(original);
					if (flag)
					{
						patchInfo2.Postfixes.DoIf(
							patchInfo => pair.Item1.Contains(patchInfo.PatchMethod.DeclaringType), 
							patchInfo => Main.Harmony.Unpatch(original, patchInfo.PatchMethod));
						patchInfo2.Prefixes.DoIf(
							patchInfo => pair.Item1.Contains(patchInfo.PatchMethod.DeclaringType),
							patchInfo => Main.Harmony.Unpatch(original, patchInfo.PatchMethod));
					}
					patchInfo2.Transpilers.DoIf(
						patchInfo => pair.Item1.Contains(patchInfo.PatchMethod.DeclaringType),
						patchInfo => Main.Harmony.Unpatch(original, patchInfo.PatchMethod));
					if (flag)
						patchInfo2.Finalizers.DoIf(
							patchInfo => pair.Item1.Contains(patchInfo.PatchMethod.DeclaringType),
							patchInfo => Main.Harmony.Unpatch(original, patchInfo.PatchMethod));
				}
			}
			patches[id] = (pair.Item1, false);
			return true;
        }
    }
}
