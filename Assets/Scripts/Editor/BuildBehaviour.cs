#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
public class BuildBehaviour
{
	[MenuItem("V+/Release Build")]
	public static void Build()
	{
		string folder= Application.dataPath + "/../" + "Build Release/";
		DirectoryInfo folderInfo = new DirectoryInfo(folder);
		if (!folderInfo.Exists) folderInfo.Create();
		string path = folder + "V+.exe";
		BuildOptions options = 0;
		options += (int)BuildOptions.CompressWithLz4; //faster?
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "RELEASE");
		PlayerSettings.usePlayerLog = false;
		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.StandaloneWindows64, options);
		PlayerSettings.usePlayerLog = true;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");
		string itemPath = Path.GetFullPath(folder).Replace(@"/", @"\");   // explorer doesn't like front slashes
		DeleteIfExists(folder + "UnityCrashHandler64.exe");
		DeleteIfExists(folder + "Data/");
		DirectoryInfo dataFolder = new DirectoryInfo(folder + "V+_Data");
		dataFolder.MoveTo(folder + "Data");
		FileInfo ffprobe = new FileInfo(folder + "Data/StreamingAssets/ffprobe.exe");
		ffprobe.MoveTo(folder + "Data/Plugins/ffprobe.exe");
		DeleteIfExists(folder + "Data/StreamingAssets");
		DirectoryInfo managedFolder = new DirectoryInfo(folder + "Data/Managed");
		foreach (FileInfo f in managedFolder.GetFiles()) if (f.Extension == ".xml") f.Delete();
		Process.Start("explorer.exe", "/select," + itemPath);
	}
	private static void DeleteIfExists(string path)
	{
		if (File.Exists(path)) File.Delete(path);
		if (Directory.Exists(path)) Directory.Delete(path,true);
	}
}
#endif