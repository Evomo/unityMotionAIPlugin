using System.Collections.Generic;
using System.IO;
using System.Linq;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;
using Application = UnityEngine.Application;
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
			
			SDKConfig sdkConfig = (SDKConfig)AssetDatabase.LoadAssetAtPath($"Assets/{Application.productName}-MotionAI.asset", typeof(SDKConfig));
			
			if (report.summary.platform == BuildTarget.iOS) // Check if the build is for iOS 
			{
				// --- configure plist
				string plistPath = report.summary.outputPath + "/Info.plist";

				PlistDocument plist = new PlistDocument(); // Read Info.plist file into memory
				plist.ReadFromString(File.ReadAllText(plistPath));

				PlistElementDict rootDict = plist.root;

				// set encryption usage for appstore upload
				rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

				if (sdkConfig.sensorType == UtilHelper.EvomoSensorType.Movesense)
				{
					rootDict.SetString("NSBluetoothAlwaysUsageDescription", "We use Bluetooth to communicate with extern motion sensors to make motion detection possible.");
					rootDict.SetString("NSBluetoothPeripheralUsageDescription", "This app requires Bluetooth to connect to an external motion sensor.");
				}
				
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
				CopyPodfile(report.summary.outputPath, sdkConfig);

			}
#endif
		}


		private static void CopyPodfile(string pathToBuiltProject, SDKConfig sdkConfig)
		
		{
			string sdkPath = PlayerSettings.productName == "EvomoUnitySDK"
				? Application.dataPath + "/MotionAI"
				: Path.GetFullPath("Packages/com.evomo.motionai");

			bool is2020 = Application.unityVersion.Contains("2020") || Application.unityVersion.Contains("2019.3");
			string suffix = $"/Core/Editor/BuildFiles/Podfile{(is2020 ? "2020" : "2019")}";
			string movesense = sdkConfig.sensorType == UtilHelper.EvomoSensorType.Movesense ? "Movesense" : "";
			string podfilePath = $"{sdkPath}{suffix}{movesense}";
			
			var destPodfilePath = pathToBuiltProject + "/Podfile";
			Debug.Log($"You selected sensorType {sdkConfig.sensorType.ToString()}");
			Debug.Log(string.Format("Copying Podfile from {0} to {1}", podfilePath, destPodfilePath));

			if (!File.Exists(destPodfilePath)) {
				FileUtil.CopyFileOrDirectory(podfilePath, destPodfilePath);
			}
			else {
				Debug.Log("Podfile already exists");
			}
			
			// Copy Unity Helper file when movesense device (because some different naming is needed)
			if (sdkConfig.sensorType == UtilHelper.EvomoSensorType.Movesense)
			{
				FileUtil.ReplaceFile($"{sdkPath}/Core/Editor/BuildFiles/UnityHelperMovesense.m", $"{pathToBuiltProject}/Libraries/MotionAI/Core/Plugins/iOS/UnityHelper.m");
			}
		}
	}
}