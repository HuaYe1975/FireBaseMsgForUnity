using UnityEngine;
using System.IO;
using UnityEditor;
using System.Xml;
using System;

public static class FirebaseManifestEditor
{
    public static string DummyManifestPath = Application.dataPath + "/Firebase/Editor/{0}_AndroidManifest.xml";
    public static string DestManifestPath = Application.dataPath + "/Plugins/Android/{0}/AndroidManifest.xml";

    static bool updateManifest(string dummyFilePath, string destPath, string newBundleID)
    {
        Debug.Log("use " + dummyFilePath);
        if (File.Exists(dummyFilePath) == false)
        {
            Debug.LogWarning("No manifest dummy file at path : " + dummyFilePath);
            return false;
        }

        string text = File.ReadAllText(dummyFilePath);
        text = text.Replace("${applicationId}", newBundleID);
        File.WriteAllText(destPath, text);
        Debug.Log("wrote " + destPath);

        return true;
    }

    public static void RefreshBundleIdTo(string newBundleID)
    {
        Debug.Log("update bundle ID : " + newBundleID);

        bool success = false;
        string dummyPath = string.Format(DummyManifestPath, "firebase-common");
        string destPath = string.Format(DestManifestPath, "firebase-common");
        success = updateManifest(dummyPath, destPath, newBundleID);

        dummyPath = string.Format(DummyManifestPath, "firebase-iid");
        destPath = string.Format(DestManifestPath, "firebase-iid");
        success |= updateManifest(dummyPath, destPath, newBundleID);

        if (success)
        {
            Debug.Log("Successfully updated manifest files.");
        }
        else
        {
            Debug.LogWarning("Firebase's Android manifest files were not updated.");
        }
    }

    [MenuItem("Firebase/Update Android manifests")]
    public static void RefreshBundleId()
    {
        RefreshBundleIdTo(PlayerSettings.bundleIdentifier);
    }
}

