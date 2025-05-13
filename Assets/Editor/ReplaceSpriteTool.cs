using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ReplaceSpriteTool", menuName = "ScriptableObject/ReplaceSpriteTool", order = 1)]
public class ReplaceSpriteTool : ScriptableObject
{
    public Sprite spriteReplace;
    public List<Sprite> spriteBase;
    public bool isSliced = true;
    public bool deleteSprite = false;


    [ContextMenu("Replace")]
    public void Replace()
    {
        List<Sprite> spriteList = new List<Sprite>();
        foreach (var sprite in spriteBase)
        {
            if (!spriteList.Contains(sprite) && sprite != spriteReplace)
            {
                spriteList.Add(sprite);
            }
        }
        if (spriteReplace != null && spriteList != null && spriteList.Count > 0)
        {
            List<string> listReference = new List<string>();
            foreach (var sprite in spriteList)
            {
                foreach (var path in Find(sprite))
                {
                    if (!listReference.Contains(path))
                    {
                        listReference.Add(path);
                    }
                }
            }
            for (int i = 0; i < listReference.Count; i++)
            {
                GameObject t = (GameObject)AssetDatabase.LoadAssetAtPath(listReference[i], typeof(GameObject));
                if (t != null)
                {
                    Image[] images = t.GetComponentsInChildren<Image>(true);
                    for (int j = 0; j < images.Length; j++)
                    {
                        if (spriteList.Contains(images[j].sprite))
                        {
                            images[j].sprite = spriteReplace;
                            if (isSliced)
                            {
                                images[j].type = Image.Type.Sliced;
                                images[j].fillCenter = true;
                            }
                        }
                    }
                    EditorUtility.SetDirty(t);
                }
            }
        }
        if (deleteSprite)
        {
            for (int j = 0; j < spriteList.Count; j++)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(spriteList[j].texture));
            }
        }
        spriteList.Clear();
        spriteBase.Clear();
    }

    private const string MenuItemText = "Assets/Find References In Project";
    private const string MenuItemDependencies = "Assets/Find Dependencies In Project";

    [MenuItem(MenuItemText, false, 25)]
    public static void FindObject()
    {
        Find(Selection.activeObject);
    }

    public static List<string> Find(Object obj)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        var referenceCache = new Dictionary<string, List<string>>();

        string[] guids = AssetDatabase.FindAssets("");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string[] dependencies = AssetDatabase.GetDependencies(assetPath, false);

            foreach (var dependency in dependencies)
            {
                if (referenceCache.ContainsKey(dependency))
                {
                    if (!referenceCache[dependency].Contains(assetPath))
                    {
                        referenceCache[dependency].Add(assetPath);
                    }
                }
                else
                {
                    referenceCache[dependency] = new List<string>() { assetPath };
                }
            }
        }

        Debug.Log("Build index takes " + sw.ElapsedMilliseconds + " milliseconds");

        string path = AssetDatabase.GetAssetPath(obj);
        List<string> listReference = new List<string>();
        if (referenceCache.ContainsKey(path))
        {
            foreach (var reference in referenceCache[path])
            {
                Debug.Log(reference, AssetDatabase.LoadMainAssetAtPath(reference));
                if (!listReference.Contains(reference))
                {
                    listReference.Add(reference);
                }
            }
        }
        else
        {
            Debug.LogWarning("No references");
        }

        referenceCache.Clear();
        return listReference;
    }

    [MenuItem(MenuItemDependencies, false, 25)]
    public static void FindDependencies()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string[] dependencies = AssetDatabase.GetDependencies(path, false);

        foreach (var dependency in dependencies)
        {
            Debug.Log(dependency, AssetDatabase.LoadMainAssetAtPath(dependency));
        }
    }

    [MenuItem(MenuItemText, true)]
    public static bool Validate()
    {
        if (Selection.activeObject)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return !AssetDatabase.IsValidFolder(path);
        }

        return false;
    }
}