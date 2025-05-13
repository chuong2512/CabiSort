#if UNITY_EDITOR
#define DEBUG
#define ASSERT
#endif

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Common
{
    //-----------------------------------
    //--------------------- Log , warning, 

    [Conditional("ENABLE_LOG")]
    public static void Log(object message)
    {
        //Log.Log(message);
    }

    [Conditional("ENABLE_LOG")]
    public static void Log(string format, params object[] args)
    {
        //Log.Log(UtilityGame.Format(format, args));
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(object message, Object context)
    {
        Debug.LogWarning(message, context);
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(Object context, string format, params object[] args)
    {
        Debug.LogWarning(UtilityGame.Format(format, args), context);
    }


    [Conditional("ENABLE_LOG")]
    public static void Warning(bool condition, object message)
    {
        if (!condition) Debug.LogWarning(message);
    }

    [Conditional("ENABLE_LOG")]
    public static void Warning(bool condition, object message, Object context)
    {
        if (!condition) Debug.LogWarning(message, context);
    }

    [Conditional("ENABLE_LOG")]
    public static void Warning(bool condition, Object context, string format, params object[] args)
    {
        if (!condition) Debug.LogWarning(UtilityGame.Format(format, args), context);
    }


    //---------------------------------------------
    //------------- Assert ------------------------

    /// Thown an exception if condition = false
    [Conditional("ASSERT")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new UnityException();
    }

    /// Thown an exception if condition = false, show message on console's log
    [Conditional("ASSERT")]
    public static void Assert(bool condition, string message)
    {
        if (!condition) throw new UnityException(message);
    }

    /// Thown an exception if condition = false, show message on console's log
    [Conditional("ASSERT")]
    public static void Assert(bool condition, string format, params object[] args)
    {
        if (!condition) throw new UnityException(UtilityGame.Format(format, args));
    }
}
