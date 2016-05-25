using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

[Serializable]
public class APIKey
{
    public string current_key;
}

[Serializable]
public class OAuthClient
{
    public string client_id;
    public string client_type;
}

[Serializable]
public class androidClientInfo
{
    public string package_name;
}

[Serializable]
public class clientInfo
{
    public string mobilesdk_app_id;
    public androidClientInfo android_client_info;
}

[Serializable]
public class Client
{
    public clientInfo client_info;
    public OAuthClient[] oauth_client;
    public APIKey[] api_key;
}

[Serializable]
public class projectInfo
{
    public string project_number;
    public string firebase_url;
    public string project_id;
    public string storage_bucket;
}

[Serializable]
public class JSonRoot
{
    public projectInfo project_info;
    public Client[] client;
    public string configuration_version;
}

public class FirebaseAssetProcessor : AssetPostprocessor
{
    private const string GoogleServiceJSON = "/Firebase/Config/google-services.json";
    public static string ConfigFirebaseJSONPath = "Assets" + GoogleServiceJSON;
    public static string ConfigFirebaseJSONPathAbsolute = Application.dataPath + GoogleServiceJSON;
    public static string FirebaseResValuePath = Application.dataPath + "/Plugins/Android/firebase-config/res/values/values.xml";

    private static void updateXMLInnerText(XmlNode node, string attributeName, string value)
    {
        if (node.Name == "string" && node.Attributes.GetNamedItem("name").Value == "default_web_client_id")
        {
            node.InnerText = value;
        }
    }

    public static void UpdateFirebaseResValues(string path)
    {
        if (File.Exists(ConfigFirebaseJSONPathAbsolute) == false)
        {
            Debug.LogError("No google service file found at : " + ConfigFirebaseJSONPathAbsolute);
            return;
        }

        string json = File.ReadAllText(ConfigFirebaseJSONPathAbsolute);
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("google services file is null or empty. " + ConfigFirebaseJSONPathAbsolute);
            return;
        }

        JSonRoot jsonRoot = JsonUtility.FromJson<JSonRoot>(json);

        XmlDocument doc = new XmlDocument();
        doc.Load(FirebaseResValuePath);
        XmlNode root = doc.DocumentElement;

        foreach (XmlNode node1 in root.ChildNodes)
        {
            updateXMLInnerText(node1, "default_web_client_id", jsonRoot.client[0].oauth_client[0].client_id);
            updateXMLInnerText(node1, "firebase_database_url", jsonRoot.project_info.firebase_url);
            updateXMLInnerText(node1, "gcm_defaultSenderId", jsonRoot.project_info.project_number);
            updateXMLInnerText(node1, "google_api_key", jsonRoot.client[0].api_key[0].current_key);
            updateXMLInnerText(node1, "google_app_id", jsonRoot.client[0].client_info.mobilesdk_app_id);
            updateXMLInnerText(node1, "google_crash_reporting_api_key", jsonRoot.client[0].api_key[0].current_key);
            updateXMLInnerText(node1, "google_storage_bucket", jsonRoot.project_info.storage_bucket);
        }

        doc.Save(FirebaseResValuePath);
        Debug.Log("Updated firebase res value file");
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var str in importedAssets)
        {
            if (str == ConfigFirebaseJSONPath)
            {
                UpdateFirebaseResValues(str);
            }
        }
    }
}