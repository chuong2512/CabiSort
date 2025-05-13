using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = ",";
    //static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    //static string LINE_SPLIT_RE = @"\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    static string comma = "|";

    public static List<Dictionary<string, string>> Read(TextAsset data)
    {
        var list = new List<Dictionary<string, string>>();
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0)
                continue;
            bool isEmptyAll = true;
            for (int j = 0; j < values.Length; j++)
            {
                if (!values[j].Equals(""))
                {
                    isEmptyAll = false;
                    break;
                }
            }
            if (isEmptyAll)
            {
                continue;
            }

            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                value = value.Replace(comma, ",");
                entry[header[j]] = value;
            }
            list.Add(entry);
        }
        return list;
    }

    public static Dictionary<string, List<string>> ReadPro(string file)
    {
        TextAsset data = Resources.Load(file) as TextAsset;
        var dic = ReadPro(data);
        return dic;
    }

    /// <summary>
    /// Key của các dictionary là header.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> ReadPro(TextAsset data)
    {
        var dic = new Dictionary<string, List<string>>();
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1)
            return dic;

        var header = Regex.Split(lines[0], SPLIT_RE);
        int length = header.Length;
        for (int i = 0; i < length; i++)
        {
            var list = new List<string>();
            if (!header[i].Equals(string.Empty) && !dic.ContainsKey(header[i]))
            {
                dic.Add(header[i], list);
            }
        }

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0)
                continue;

            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                value = value.Replace(comma, ",");
                dic[header[j]].Add(value);
            }
        }
        return dic;
    }
    
    public static List<Dictionary<string, string>> Read(string content, bool lowerCaseKey = false)
    {
        return ReadSpecialSplit(content, lowerCaseKey, SPLIT_RE);
    }
    
    public static List<Dictionary<string, string>> ReadSpecialSplit(string content, bool lowerCaseKey = false , string keySplit = ",")
    {
        var list = new List<Dictionary<string, string>>();
        var lines = Regex.Split(content, LINE_SPLIT_RE);
        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], keySplit);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], keySplit);
            if (values.Length == 0)
                continue;

            bool isEmptyAll = true;
            for (int j = 0; j < values.Length; j++)
            {
                if (!values[j].Equals(""))
                {
                    isEmptyAll = false;
                    break;
                }
            }
            if (isEmptyAll)
            {
                continue;
            }

            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                value = value.Replace(comma, ",");
                var key = lowerCaseKey ? header[j].ToLower() : header[j];
                key = key.TrimStart(' ').TrimEnd(' ');
                entry[key] = value;
            }
            list.Add(entry);
        }
        return list;
    }
}
