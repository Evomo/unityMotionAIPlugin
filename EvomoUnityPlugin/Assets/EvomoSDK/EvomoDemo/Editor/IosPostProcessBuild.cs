using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class IosPostprocess : IPostprocessBuildWithReport // Will execute after XCode project is built
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
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
            //tring[] targets = {pbxProject.TargetGuidByName("Unity-iPhone")};
            // for 2020
            string[] targets = { pbxProject.GetUnityMainTargetGuid(), pbxProject.GetUnityMainTargetGuid() };


            // set swift version for cocoapod install
            pbxProject.SetBuildProperty(targets, "SWIFT_VERSION", "5");

            // Apply settings
            File.WriteAllText(projectPath, pbxProject.WriteToString());


            // --- copy podfile to project folder
            CopyPodfile(report.summary.outputPath);
        }
    }

    private static void CopyPodfile(string pathToBuiltProject)
    {

        var podfilePath = Application.dataPath + "/EvomoSDK/BuildFiles/Podfile2019";

        if (Application.unityVersion.Contains("2020"))
        {
            podfilePath = Application.dataPath + "/EvomoSDK/BuildFiles/Podfile2020";
        }

        var destPodfilePath = pathToBuiltProject + "/Podfile";

        Debug.Log(string.Format("Copying Podfile from {0} to {1}", podfilePath, destPodfilePath));

        if (!File.Exists(destPodfilePath))
        {
            FileUtil.CopyFileOrDirectory(podfilePath, destPodfilePath);
        }
        else
        {
            Debug.Log("Podfile already exists");
        }
    }
}