
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class GameHelper
{
    public static Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 90, 0));
    public static Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    public static float GetRatio()
    {
        float f = (float)720 / (float)1280;
        float t = Camera.main.aspect;

        return t / f;
    }

    public static void DeleteAllChilds(this Transform t)
    {
        int count = t.childCount;
        for (int i = 0; i < count; i++)
            Object.Destroy(t.GetChild(i).gameObject);
    }

    public static void HideAllChilds(this Transform t)
    {
        int count = t.childCount;
        for (int i = 0; i < count; i++)
            t.GetChild(i).gameObject.Hide();
    }

    public static void DeleteDestroyImmediateAllChilds(this Transform t)
    {
        int count = t.childCount;
        for (int i = 0; i < count; i++)
            Object.DestroyImmediate(t.GetChild(0).gameObject);
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, digits);
        return Mathf.Round(value * mult) / mult;
    }
    /// <summary>
    /// Return list object in scene (without active)
    /// </summary>
    /// <returns></returns>
    public static List<Object> GetAllObjectsInScene()
    {
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();
        List<Object> listObjs = new List<Object>();
        foreach (var item in objs)
            FindAllChild(item.transform, ref listObjs);
        return listObjs;
    }
    /// <summary>
    /// Find child in <see cref="Transform"/>
    /// </summary>
    /// <param name="t">Trans need to find child</param>
    /// <param name="list">List all child of transform</param>
    private static void FindAllChild(Transform t, ref List<Object> list)
    {
        list.Add(t.gameObject);
        foreach (Transform child in t)
            FindAllChild(child, ref list);
    }

    public static void SetSizeFollowWidth(this Image img, int maxWidth)
    {
        if (img.sprite == null)
            return;
        float aspect = img.sprite.bounds.size.y / img.sprite.bounds.size.x;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, maxWidth * aspect);
    }

    public static void SetSizeFollowHeight(this Image img, int maxHeight)
    {
        if (img.sprite == null)
            return;
        float aspect = img.sprite.bounds.size.y / img.sprite.bounds.size.x;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(maxHeight / aspect, maxHeight);
    }

    public static Vector3 GetSizeScaleOfSprite(SpriteRenderer sprite)
    {
        Vector3 sizeScale;
        var size = Camera.main.orthographicSize;

        var worldScreenHeight = size * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var boxSize = sprite.sprite.bounds.size.x;
        sizeScale = Vector3.one * (float)(worldScreenWidth / size) / boxSize;

        return sizeScale;
    }

    // private static int HexToDec(string hex)
    // {
    //     int dec = System.Convert.ToInt32("FF", 16);
    //     return dec;
    // }

    // private static float HexToFloatNormalized(string hex)
    // {
    //     return HexToDec(hex) / 255f;
    // }

    public static Color GetColorFromString(string hex)
    {
        // float red = HexToFloatNormalized(hex.Substring(0, 2));
        // float green = HexToFloatNormalized(hex.Substring(2, 2));
        // float blue = HexToFloatNormalized(hex.Substring(4, 2));

        Color color;

        ColorUtility.TryParseHtmlString(hex, out color);

        return color;
    }

#if UNITY_EDITOR

    public static List<T> GetAllAssetAtPath<T>(string filter, string path)
    {
        string[] findAssets = UnityEditor.AssetDatabase.FindAssets(filter, new[] { path });
        List<T> os = new List<T>();
        foreach (var findAsset in findAssets)
        {
            os.Add((T)Convert.ChangeType(UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(findAsset), typeof(T)), typeof(T)));
        }
        return os;
    }

    public static List<string> GetAllScenes()
    {
        List<string> scenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            scenes.Add(SceneManager.GetSceneAt(i).name);
        return scenes;
    }

    public static List<Object> GetAllAssets(string path)
    {
        string[] paths = { path };
        var assets = UnityEditor.AssetDatabase.FindAssets(null, paths);
        var assetsObj = assets.Select(s => UnityEditor.AssetDatabase.LoadMainAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(s))).ToList();
        return assetsObj;
    }

    public static void PingObj(string path)
    {
        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);
        UnityEditor.Selection.activeObject = obj;
        UnityEditor.EditorGUIUtility.PingObject(obj);
    }
#endif
}

public static class Extension
{
    public static void Hide(this GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void Hide(this Component component)
    {
        component.gameObject.SetActive(false);
    }

    public static void Show(this GameObject obj)
    {
        obj.SetActive(true);
    }

    public static void Show(this Component o)
    {
        o.gameObject.SetActive(true);
    }

    public static T Cast<T>(this MonoBehaviour mono) where T : class
    {
        var t = mono as T;
        return t;
    }
}
