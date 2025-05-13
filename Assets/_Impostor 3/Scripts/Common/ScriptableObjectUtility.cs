#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static T LoadOrCreateAsset<T>(string resourceFolderPath, string resourcePath, string name) where T : ScriptableObject
    {
        string pathfFromResource = resourcePath + name;
        T loadObj = Resources.Load<T>(pathfFromResource);
        if (loadObj == null)
        {
            string folderPath = resourceFolderPath + resourcePath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            T newAsset = ScriptableObject.CreateInstance<T>();
            string assetPathAndName = resourceFolderPath + pathfFromResource + ".asset";
            AssetDatabase.CreateAsset(newAsset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return LoadOrCreateAsset<T>(resourceFolderPath, resourcePath, name);
        }
        else
            return loadObj;
    }
}
#endif