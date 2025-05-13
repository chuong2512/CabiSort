using System.Diagnostics;
using UnityEngine;
using System;

[System.Serializable]
public class TimeInGame : IComparable<TimeInGame>
{
    public int Day;
    public int Month;
    public int Year;
    public int Minutes;
    public int Hours;
    public int Second;

    public TimeInGame()
    {
        var dataTime = DateTime.MaxValue;
        Day = dataTime.Day;
        Month = dataTime.Month;
        Year = dataTime.Year;
        Minutes = dataTime.Minute;
        Hours = dataTime.Hour;
        Second = dataTime.Second;
    }

    public TimeInGame(DateTime dateTime)
    {
        Day = dateTime.Day;
        Month = dateTime.Month;
        Year = dateTime.Year;
        Minutes = dateTime.Minute;
        Hours = dateTime.Hour;
        Second = dateTime.Second;
    }

    public static int GetYear(TimeInGame c1, TimeInGame c2)
    {
        return (c2.Year - c1.Year);
    }

    public static int GetMonth(TimeInGame c1, TimeInGame c2)
    {
        return (GetYear(c1, c2) * 12 + (c2.Month - c1.Month));
    }

    public static int GetDay(TimeInGame c1, TimeInGame c2)
    {
        return GetMonth(c1, c2) * 30 + (c2.Day - c1.Day);
    }

    public static int GetHours(TimeInGame c1, TimeInGame c2)
    {
        return GetDay(c1, c2) * 24 + (c2.Hours - c1.Hours);
    }

    public static int GetMinutes(TimeInGame c1, TimeInGame c2)
    {
        return GetHours(c1, c2) * 60 + (c2.Minutes - c1.Minutes);
    }

    public static int GetSecond(TimeInGame c1, TimeInGame c2)
    {
        return GetMinutes(c1, c2) * 60 + (c2.Second - c1.Second);
    }

    public TimeInGame(int year, int month, int day, int hours, int minutes, int second)
    {
        Day = day;
        Month = month;
        Year = year;
        Minutes = minutes;
        Hours = hours;
        Second = second;
    }

    public void SetTime(int day, int month, int year, int minutes = 0, int hours = 0, int second = 0)
    {
        Day = day;
        Month = month;
        Year = year;
        Minutes = minutes;
        Hours = hours;
        Second = second;
    }

    public void SetTime(DateTime dataTime)
    {
        SetTime(dataTime.Day, dataTime.Month, dataTime.Year, dataTime.Minute, dataTime.Hour, dataTime.Second);
    }

    public bool IsSameDay(DateTime dataTime)
    {
        if (Day == dataTime.Day && Month == dataTime.Month && Year == dataTime.Year)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CountDays()
    {
        System.TimeSpan deltaTime = (System.DateTime.Now - (new System.DateTime(this.Year, this.Month, this.Day)));
        return (float)deltaTime.TotalDays;
    }

    public float CountMinutes()
    {
        System.TimeSpan deltaTime = (System.DateTime.Now - (new System.DateTime(this.Year, this.Month, this.Day)));
        return (float)deltaTime.TotalMinutes;
    }
    
    // public float CountSeconds()
    // {
    //     TimeSpan deltaTime = UtilityGame.CurrentTimeServerNowTimeZoneZero - new DateTime(this.Year, this.Month, this.Day,this.Hours,this.Minutes,this.Second);
    //     return (float)deltaTime.TotalSeconds;
    // }
    
    public int CompareTo(TimeInGame other)
    {
        var thisDate = new DateTime(Year, Month, Day, Hours, Minutes, Second);
        var otherDate = new DateTime(other.Year, other.Month, other.Day, other.Hours, other.Minutes, other.Second);
        return thisDate.CompareTo(otherDate);
    }

    public DateTime DateTime
    {
        get
        {
            try
            {
                return new DateTime(Year, Month, Day, Hours, Minutes, Second);
            }
            catch (Exception ex)
            {
                Log.Warning(ex);
                return DateTime.Now;
            }
        }
    }

    public override string ToString()
    {
        return Day.ToString() + "/" + Month.ToString() + "/" + Year.ToString();
    }
}

