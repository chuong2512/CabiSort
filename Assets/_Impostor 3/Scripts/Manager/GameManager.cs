using System;
using CodeStage.AntiCheat.Storage;
using Zitga.CsvTools.Tutorials;

public enum TypeScene
{
    MainMenu,
    Gameplay,
}

public class GameManager : SingletonMonoDontDestroy<GameManager>
{
    [System.NonSerialized] public DataCreatePlayer dataCreatePlayer;
    public bool isRemoveAds = false;
    public bool isTest;

    public int currentLevel=0;

    public FirstLevels firstLevels;
    public RandomLevels randomLevels;

    public int numberRewinds
    {
        get
        {
            return ObscuredPrefs.GetInt("Rewind", 5);
        }

        set
        {
            ObscuredPrefs.SetInt("Rewind", value);
            ObscuredPrefs.Save();
        }
    }


    public bool isAddBox = false;
    
    public bool IsEditor
    {
        get
        {
#if UNITY_EDITOR
            return true;
#endif
            return false;
        }
    }
    
    public void SaveDataCreatePlayer()
    {
        UtilityGame.SaveUserData(dataCreatePlayer);
        ObscuredPrefs.Save();
    }
    
    private void OnApplicationQuit()
    {
        SaveDataCreatePlayer();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveDataCreatePlayer();
        }
    }
}
