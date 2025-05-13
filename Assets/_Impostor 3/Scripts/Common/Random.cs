using System;

public static class RandNumber
{
    private static System.Random rand
    {
        get
        {
            if (_rand == null)
            {
                ResetRandom();
            }

            return _rand;
        }
    }
    private static System.Random _rand;
    private static bool isLog = false;
    public static void ResetRandom()
    {
        //isLog = true;
        _rand = new System.Random((int)DateTime.Now.Ticks);
    }

    public static int Random(int inclusiveMin, int exclusiveMax)
    {
        int value = rand.Next(inclusiveMin, exclusiveMax);
        return value;
    }

    public static float Random(float inclusiveMin, float inclusiveMax)
    {
        float value = (float)(rand.NextDouble() * (inclusiveMax - inclusiveMin) + inclusiveMin);
        if (isLog)
            Log.Info(value);
        return value;
    }

    public static float Random01()
    {
        return Random(0f, 1f);
    }
}
