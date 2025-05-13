using CodeStage.AntiCheat.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UtilityGame
{
    public const string DataCreatePlayerObscured = "dataCreatePlayerObscured";
    
    public static bool IsGamePause;

    public static void PauseGame()
    {
        Time.timeScale = 0;
        IsGamePause = true;
    }

    public static void UnPauseGame()
    {
        Time.timeScale = 1;
        IsGamePause = false;
    }

    public static bool IsNoneConnectInternet
    {
        get
        {
            return  Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
    
    public static DataCreatePlayer LoadDataCreatePlayer()
    {
        string jsonDataObscured = ObscuredPrefs.GetString(DataCreatePlayerObscured);
        if (jsonDataObscured != "")
        {
            if (jsonDataObscured.Length > 0)
            {
                return JsonUtility.FromJson<DataCreatePlayer>(jsonDataObscured);
            }
        }
        return new DataCreatePlayer();
    }

    public static void SaveUserData(DataCreatePlayer userData)
    {
        string jsonData = JsonUtility.ToJson(userData);
        ObscuredPrefs.SetString(DataCreatePlayerObscured, jsonData);
    }

    public static String Format(String format, params object[] args)
    {
        try
        {
            return string.Format(format, args);
        }
        catch
        {
            return "Error Content!";
        }
    }
    
    public static DateTime ConvertStringTime(string time)
    {
        int year = int.Parse(time.Substring(0, 4));
        int month = int.Parse(time.Substring(4, 2));
        int day = int.Parse(time.Substring(6, 2));
        int hour = int.Parse(time.Substring(8, 2));
        int minute = int.Parse(time.Substring(10, 2));
        int second = int.Parse(time.Substring(12, 2));
        return new DateTime(year, month, day, hour, minute, second);
    }
    
    public static string ConvertKMT(float num)
    {
        if (num < 1000000f)
        {
            if (num >= 10000f)
            {
                num = ((long)(num / 100)) * 100;
                return (num / 1000f).ToString("0.#") + "K";
            }
            else
            {
                return num.ToString();
            }
        }
        else if (num < 1000000000f)
        {
            num = ((long)(num / 100000)) * 100000;
            return (num / 1000000f).ToString("0.#") + "M";
        }
        else if (num < 1000000000000f)
        {
            num = ((long)(num / 100000000)) * 100000000;
            return (num / 1000000000f).ToString("0.#") + "B";
        }
        else
        {
            num = ((long)(num / 100000000000)) * 100000000000;
            return (num / 1000000000000f).ToString("0.#") + "T";
        }
    }
}
