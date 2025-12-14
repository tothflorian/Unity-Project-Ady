using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private GameManager gameManager;
    private System.Random _random; 
    private int _currentDay;
    private int _currentWeek;

    public TextMeshProUGUI currentDayText;

    void Start()
    {
        _random = new System.Random();

        _currentDay = 0;
        _currentWeek = 0;
        currentDayText.text = "Day 0";
    }

    public void PassTime()
    {
        _currentDay += PassAmount();
        currentDayText.text = "Day " + _currentDay.ToString();

        if (_currentDay / 7 > _currentWeek)
        {
            _currentWeek++;
            gameManager.WeekPassed();
        }




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
