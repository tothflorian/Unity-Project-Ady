using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private System.Random _random; 

    public int currentDay;

    void Start()
    {
        currentDay = 0;
    }

    public void PassTime()
    {
        currentDay += PassAmount();

        // Egyéb game logic pl. animációk (talán egy check, hogy változott-e olyan, amit mutatni kell a játékosnak is)
    }

    private int PassAmount()
    {
        int __chance = _random.Next(1, 101);

        if (__chance < 70)
            return 1;
        else if (__chance < 90)
            return 2;
        else
            return 3;
    }
}
