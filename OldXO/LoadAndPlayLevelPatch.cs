using BackToThePast.Patch;
using System.IO;
using UnityEngine;

namespace BackToThePast.OldXO
{
    [BTTPPatch(typeof(CustomLevel), "LoadAndPlayLevel")]
    public static class LoadAndPlayLevelPatch
    {
        public static bool Prefix(CustomLevel __instance, string levelPath, ref bool __result)
        {
			if (levelPath != "BackToThePast.OldXO")
				return true;
			scnEditor editor = scnEditor.instance;
			__instance.FlushUnusedMemory();
			Resources.UnloadUnusedAssets();
			__instance.isLoading = true;
			__instance.levelPath = levelPath;
            __instance.levelData.Decode(Main.oldXo, out _);
			//__instance.levelData.backgroundSettings["showDefaultBGIfNoImage"] = ToggleBool.Disabled;
			editor.filenameText.text = Path.GetFileName(levelPath);
			editor.filenameText.fontStyle = FontStyle.Bold;
			scrConductor.instance.SetupConductorWithLevelData(__instance.levelData);
			__instance.RemakePath(true);
			__instance.UpdateFloorSprites();
			__instance.SetBackground();
			__instance.imgHolder.Unload(true);
			scrConductor.instance.song.clip = Main.oneForgottenNight;
			DiscordController discordController = DiscordController.instance;
			if (discordController != null)
				discordController.UpdatePresence();
			__instance.Play(0);
			foreach (Transform child in __instance.editorBG.transform)
				if (child.name.StartsWith("opt_tutorial_grayscale_"))
					child.gameObject.SetActive(false);
			__result = true;
			return false;
		}
    }
}
