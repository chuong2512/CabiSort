using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptimizeBorderSprite
{
    [MenuItem("Assets/OptimizeSprite")]
    private static void OptimizeSprite()
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (var obj in Selection.objects)
        {
            Sprite sprite = null;
            if (obj is Sprite)
            {
                sprite = obj as Sprite;
            }
            else if (obj is Texture2D)
            {
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(obj));
            }
            else
            {
                Debug.Log("Not support type " + obj.GetType().ToString());
            }
            if (sprite != null && ((sprite.border.x > 0 && sprite.border.z > 0) || (sprite.border.y > 0 && sprite.border.w > 0)))
            {
                string path = AssetDatabase.GetAssetPath(sprite.texture);
                int pixelX = sprite.texture.width;
                int pixelY = sprite.texture.height;
                int width = pixelX;
                int height = pixelY;
                int bottom = 0;
                int left = 0;
                if (sprite.border.x > 0 && sprite.border.z > 0)
                {
                    left = (int)sprite.border.x;
                    width = (int)(sprite.border.x + sprite.border.z);
                }
                if (sprite.border.y > 0 && sprite.border.w > 0)
                {
                    bottom = (int)sprite.border.y;
                    height = (int)(sprite.border.y + sprite.border.w);
                }
                Texture2D texture = new Texture2D(width, height);
                int size = width * height;
                TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
                if (!ti.isReadable)
                {
                    ti.isReadable = true;
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
                Color[] pixel = sprite.texture.GetPixels();
                Color[] pixelNew = new Color[size];
                int c, r;
                for (int i = 0; i < size; i++)
                {
                    c = i % width;
                    r = i / width;
                    if (c >= left)
                    {
                        c = pixelX - (width - c);
                    }
                    if (r >= bottom)
                    {
                        r = pixelY - (height - r);
                    }
                    pixelNew[i] = pixel[r * pixelX + c];
                }
                texture.SetPixels(pixelNew);
                string pathSystem = Application.dataPath;
                pathSystem = pathSystem.Substring(0, pathSystem.Length - 6) + AssetDatabase.GetAssetPath(sprite);
                texture.Apply();
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(pathSystem, bytes);
                sprites.Add(sprite);
            }
        }
        AssetDatabase.Refresh();
        if (sprites.Count > 0)
        {
            FixImageSprite(sprites);
        }
    }

    private static void FixImageSprite(List<Sprite> sprites)
    {
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
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (Object child in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(child);
            if (referenceCache.ContainsKey(path))
            {
                foreach (var reference in referenceCache[path])
                {
                    GameObject gmObject = AssetDatabase.LoadAssetAtPath(reference, (typeof(GameObject))) as GameObject;
                    if (gmObject != null && !gameObjects.Contains(gmObject))
                    {
                        gameObjects.Add(gmObject);
                    }
                }
            }
        }
        List<Image> imgs;
        foreach (GameObject gm in gameObjects)
        {
            imgs = GetImage(gm.transform, sprites);
            foreach (Image im in imgs)
            {
                if (im.type == Image.Type.Simple)
                {
                    im.type = Image.Type.Sliced;
                }
            }
            imgs.Clear();
        }
        gameObjects.Clear();
        referenceCache.Clear();
    }

    public static List<Image> GetImage(Transform trans, List<Sprite> sprites)
    {
        var result = new List<Image>();
        bool active = trans.gameObject.activeSelf;
        trans.gameObject.SetActive(true);
        var im = trans.GetComponent<Image>();
        if (im != null && im.sprite != null && sprites.Contains(im.sprite))
        {
            result.Add(im);
        }
        foreach (Transform child in trans)
        {
            result.AddRange(GetImage(child, sprites));
        }
        trans.gameObject.SetActive(active);
        return result;
    }

    [MenuItem("Assets/FindTexturePVRTC")]
    static void FindTexturePVRTC()
    {
        if (Selection.activeObject != null)
        {
            string[] guids2 = AssetDatabase.FindAssets("t:texture2D", new[] { AssetDatabase.GetAssetPath(Selection.activeObject) });
            List<Object> gos = new List<Object>();
            foreach (string guid2 in guids2)
            {
                if (AssetDatabase.GUIDToAssetPath(guid2).Contains("/PVRTC/"))
                {
                    Object go = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid2));
                    gos.Add(go);
                }
            }
            Selection.objects = gos.ToArray();
        }
    }

    [MenuItem("Assets/FindTextureASTC")]
    static void FindTextureASTC()
    {
        if (Selection.activeObject != null)
        {
            string[] guids2 = AssetDatabase.FindAssets("t:texture2D", new[] { AssetDatabase.GetAssetPath(Selection.activeObject) });
            List<Object> gos = new List<Object>();
            foreach (string guid2 in guids2)
            {
                if (!AssetDatabase.GUIDToAssetPath(guid2).Contains("/PVRTC/"))
                {
                    Object go = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid2));
                    gos.Add(go);
                }
            }
            Selection.objects = gos.ToArray();
        }
    }
}
