using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zitga.CsvTools.Tutorials
{
    public class FirstLevels : ScriptableObject
    {
        [Serializable]
        public class Boxes
        {
            public int[] box1;
        }
    
        [Serializable]
        public class FirstLevelsData
        {
            // public int level;
            public int emptyBox;
            public int fillBox;
            public int totalBox;
            public Boxes[] boxes;
        }
    
        public FirstLevels.FirstLevelsData[] firstLevelsDatas;
    }
    
#if UNITY_EDITOR
    public class FirstLevelsPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                if (str.IndexOf("/first_levels.csv", StringComparison.Ordinal) != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetFile = str.Replace(".csv", ".asset");
                    FirstLevels gm = AssetDatabase.LoadAssetAtPath<FirstLevels>(assetFile);
                    if (gm == null)
                    {
                        gm = ScriptableObject.CreateInstance<FirstLevels>();
                        AssetDatabase.CreateAsset(gm, assetFile);
                    }
    
                    gm.firstLevelsDatas = CsvReader.Deserialize<FirstLevels.FirstLevelsData>(data.text);
    
                    EditorUtility.SetDirty(gm);
                    AssetDatabase.SaveAssets();
                    Debug.Log("Reimport Asset: " + str);
                }
            }
        }
    }
#endif
}
