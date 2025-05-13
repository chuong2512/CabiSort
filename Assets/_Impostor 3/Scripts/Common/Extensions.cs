using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static int Replace<T>(this IList<T> source, T oldValue, T newValue)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var index = source.IndexOf(oldValue);
        if (index != -1)
            source[index] = newValue;
        return index;
    }

    public static void ReplaceAll<T>(this IList<T> source, T oldValue, T newValue)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        int index = -1;
        do
        {
            index = source.IndexOf(oldValue);
            if (index != -1)
                source[index] = newValue;
        } while (index != -1);
    }


    public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        return source.Select(x => EqualityComparer<T>.Default.Equals(x, oldValue) ? newValue : x);
    }

    public static void StartDelayMethod(this MonoBehaviour mono, float time, Action callback)
    {
        mono.StartCoroutine(Delay(time, callback));
    }

    static IEnumerator Delay(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback.Invoke();
        }
        else
        {
            Log.Info("Call back is destroyed");
        }
    }


    public static void StartDelayMethodRealTime(this MonoBehaviour mono, float time, Action callback)
    {
        mono.StartCoroutine(DelayReal(time, callback));
    }

    static IEnumerator DelayReal(float time, Action callback)
    {
        float end = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < end)
        {
            yield return null;
        }
        if (callback != null)
        {
            callback.Invoke();
        }
        else
        {
            Log.Info("Call back is destroyed");
        }
    }
}