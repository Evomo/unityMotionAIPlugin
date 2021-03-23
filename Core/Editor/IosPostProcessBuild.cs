using System.Collections.Generic;
using System.IO;
using System.Linq;
using MotionAI.Core.Controller;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif
namespace MotionAI.Core.Editor {
	public class IosPostprocess : IPostprocessBuildWithReport // Will execute after XCode project is built
	{
		public int callbackOrder {
			get { return 0; }
		}

		public void OnPostprocessBuild(BuildReport report) {
#if UNITY_EDITOR_OSX
			if (report.summary.platform == BuildTarget.iOS) // Check if the build is for iOS 
			{
				// --- configure plist
				string plistPath = report.summary.outputPath + "/Info.plist";

				PlistDocument plist = new PlistDocument(); // Read Info.plist file into memory
				plist.ReadFromString(File.ReadAllText(plistPath));

				PlistElementDict rootDict = plist.root;

				// set encryption usage for appstore upload
				rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

				File.WriteAllText(plistPath, plist.WriteToString()); // Override Info.plist

				// ---- configure build settings

				// Initialize PbxProject
				var projectPath = report.summary.outputPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
				PBXProject pbxProject = new PBXProject();
				pbxProject.ReadFromFile(projectPath);

				// for 2019
				//string[] targets = {pbxProject.TargetGuidByName("Unity-iPhone")};
				// for 2020
				string[] targets = {pbxProject.GetUnityMainTargetGuid(), pbxProject.GetUnityMainTargetGuid()};


				// set swift version for cocoapod install
				pbxProject.SetBuildProperty(targets, "SWIFT_VERSION", "5");

				// Apply settings
				File.WriteAllText(projectPath, pbxProject.WriteToString());


				// --- copy podfile to project folder
				CopyPodfile(report.summary.outputPath);

			}
#endif
		}


		private static void CopyPodfile(string pathToBuiltProject)
		{

			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				Debug.Log(scene.path);
			}

			Debug.Log("sdfs2222dfsdf");
			GameObject motionManagerGO = GameObject.Find("MotionAIManager");
			Debug.Log(motionManagerGO.ToString());
			//MotionAIManager manager = motionManagerGO.GetComponent<MotionAIManager>();
			//Debug.Log(manager.deviceType.ToString());
			
			
			
			string prefix = PlayerSettings.productName == "unityMotionAIPlugin"
				? Application.dataPath + "/MotionAI"
				: Path.GetFullPath("Packages/com.evomo.motionai");

			bool is2020 = Application.unityVersion.Contains("2020") || Application.unityVersion.Contains("2019.3");
			string suffix = $"/Core/Editor/BuildFiles/Podfile{(is2020 ? "2020" : "2019")}";
			string podfilePath = $"{prefix}{suffix}";


			var destPodfilePath = pathToBuiltProject + "/Podfile";

			Debug.Log(string.Format("Copying Podfile from {0} to {1}", podfilePath, destPodfilePath));

			if (!File.Exists(destPodfilePath)) {
				FileUtil.CopyFileOrDirectory(podfilePath, destPodfilePath);
			}
			else {
				Debug.Log("Podfile already exists");
			}
		}
	}
}