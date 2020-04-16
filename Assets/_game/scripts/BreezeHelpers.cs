using System;
using System.Collections.Generic;

public static class BreezeHelpers
{
    
   public static void ShuffleList<T>(List<T> list)
    {

        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
	
	public static int Fib(int n)
    {
        return (n < 2) ? n : Fib(n - 1) + Fib(n - 2);
    }
	
public static string TimeFromFloat(float val)
    {
        TimeSpan time = TimeSpan.FromSeconds(val);
        string temp = string.Empty;
        if (val > 3600f)
        {
            temp = string.Format("{0}h:{1}m:{2}s", time.Hours, time.Minutes, time.Seconds);
        }
        else if(val > 60f){
            temp = string.Format("{0}m:{1}s", time.Minutes, time.Seconds);
        }
        else
        {
            temp = string.Format("{0}s",  time.Seconds);
        }
        return temp;
    }
}

