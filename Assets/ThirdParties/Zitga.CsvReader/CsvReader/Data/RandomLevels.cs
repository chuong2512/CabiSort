using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Zitga.CsvTools.Tutorials
{
    public class RandomLevels : ScriptableObject
    {
        [Serializable]
        public class RandomLevelsData
        {
            // public int level;
            public int emptyBox;
            public int fillBox;
            public int totalBox;
            public int numberColors;
        }

        public RandomLevels.RandomLevelsData[] randomlevelsDatas;

#if UNITY_EDITOR
        public class FirstLevelsPostprocessor : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                string[] movedFromAssetPaths)
            {
                foreach (string str in importedAssets)
                {
                    if (str.IndexOf("/random_levels.csv", StringComparison.Ordinal) != -1)
                    {
                        TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                        string assetFile = str.Replace(".csv", ".asset");
                        RandomLevels gm = AssetDatabase.LoadAssetAtPath<RandomLevels>(assetFile);
                        if (gm == null)
                        {
                            gm = ScriptableObject.CreateInstance<RandomLevels>();
                            AssetDatabase.CreateAsset(gm, assetFile);
                        }

                        gm.randomlevelsDatas = CsvReader.Deserialize<RandomLevels.RandomLevelsData>(data.text);

                        EditorUtility.SetDirty(gm);
                        AssetDatabase.SaveAssets();
                        Debug.Log("Reimport Asset: " + str);
                    }
                }
            }
        }
#endif
    }
}