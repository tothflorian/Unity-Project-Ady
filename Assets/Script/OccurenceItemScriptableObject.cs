using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OccurenceItem")]
public class OccurenceItemScriptableObject : ScriptableObject
{
    
    public static string Occurence;

    private static int iDaysPassed;

    public static int IDaysPassed
    {
        set
        {
            iDaysPassed += value;
            if (iDaysPassed == 0)
                SDaysPassed = "Today";
            else if (iDaysPassed == 1)
                SDaysPassed = "Yesterday";
            else
                SDaysPassed = iDaysPassed + " days ago";
        }
    }
    
    public static string SDaysPassed { get; private set;}
}
