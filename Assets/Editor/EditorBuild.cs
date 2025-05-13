using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
namespace Assets.Editor
{
    public class EditorBuild
    {
        //[MenuItem("Project/Load Data CSV")]
        //private static void LoadDataCSV()
        //{
        //    var prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/LoadDataHelper.prefab", typeof(GameObject));
        //    var loadDataHelperPrefab = Object.Instantiate(prefab) as GameObject;
        //    var loadDataHelperScript = loadDataHelperPrefab.GetComponent<LoadDataHelper>();
        //    loadDataHelperScript.LoadAllData();
        //}

        [MenuItem("Project/ClearData")]
        private static void DeletePlayerPref()
        {
            //PlayerPrefs.DeleteAll();
            //var path = Application.persistentDataPath + "/" + UtilityGame.DataCreatePlayerObscured + ".json";
            //File.WriteAllText(path, "");
        }
    }
}