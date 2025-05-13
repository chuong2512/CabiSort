using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVHelper : MonoBehaviour
{
    private const int NUMBER_RESOURCE_MAX = 20;

    public static List<List<Dictionary<string, string>>> getListGroupCSV(List<Dictionary<string, string>> csvData, string tagSeparator)
    {
        List<List<Dictionary<string, string>>> result = new List<List<Dictionary<string, string>>>();
        List<Dictionary<string, string>> csvListOfElement = new List<Dictionary<string, string>>();
        int groupIdentify = -1;
        foreach (Dictionary<string, string> row in csvData)
        {
            if (row.ContainsKey(tagSeparator) && !row[tagSeparator].Equals(""))
            {
                string tag = row[tagSeparator].Trim();
                if (!tag.Equals(""))
                {
                    int groupIdentifyNext = int.Parse(row[tagSeparator]);

                    if (groupIdentify == -1)
                    {
                        groupIdentify = groupIdentifyNext;
                    }

                    if (groupIdentifyNext != groupIdentify)
                    {
                        result.Add(csvListOfElement);
                        csvListOfElement = new List<Dictionary<string, string>>();
                    }
                }
            }

            csvListOfElement.Add(row);
        }

        if (groupIdentify != -1)
        {
            result.Add(csvListOfElement);
        }

        if (result.Count == 0)
        {
            throw new Exception("result must not null");
        }

        return result;
    }
    
    public static int GetIdFromFileName(string filename)
    {
        var splitString = filename.Split('_');
        return int.Parse(splitString[1]);
    }
    
    public static List<List<Dictionary<string, string>>> GetListGroupCSV(List<Dictionary<string, string>> csvData,
        string tagSeparator)
    {
        List<List<Dictionary<string, string>>> result = new List<List<Dictionary<string, string>>>();
        List<Dictionary<string, string>> csvListOfElement = new List<Dictionary<string, string>>();
        string groupIdentify = string.Empty;
        foreach (Dictionary<string, string> row in csvData)
        {
            if (row.ContainsKey(tagSeparator) && !row[tagSeparator].Equals(""))
            {
                string groupIdentifyNext = row[tagSeparator].Trim();
                if (!string.IsNullOrEmpty(groupIdentifyNext))
                {
                    if (string.IsNullOrEmpty(groupIdentify))
                    {
                        groupIdentify = groupIdentifyNext;
                    }

                    // Debug.Log("X "+groupIdentifyNext + " " + groupIdentify);
                    if (!groupIdentifyNext.Equals(groupIdentify))
                    {
                        result.Add(csvListOfElement);
                        csvListOfElement = new List<Dictionary<string, string>>();

                        groupIdentify = groupIdentifyNext;
                    }
                }
            }

            csvListOfElement.Add(row);
        }

        if (!string.IsNullOrEmpty(groupIdentify))
        {
            result.Add(csvListOfElement);
        }

        if (result.Count == 0)
        {
            Debug.LogWarning("Load Group csv count = 0");
            return new List<List<Dictionary<string, string>>> { csvData };
        }

        return result;
    }
   
}
