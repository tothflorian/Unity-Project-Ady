using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}
    private System.Random _random; 
    public int _currentDay;
    private int _currentWeek;

    private TextMeshProUGUI currentDayText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _random = new System.Random();

        _currentDay = 0;
        _currentWeek = 0;
    }
    public void BindUI(int? savedDay = null)
    {
        if (savedDay != null)
        {
            _currentDay = savedDay.Value;
        }
        currentDayText = GameObject.Find("Day").transform.GetComponent<TextMeshProUGUI>();
        currentDayText.text = "Day " + _currentDay;
    }

    public void PassTime()
    {
        int passAmount = PassAmount();
        _currentDay += passAmount;
        currentDayText.text = "Day " + _currentDay.ToString();

        List<OccurenceEntry> occurences = GameManager.Instance.occurenceList;
        occurences.Reverse();

        foreach(OccurenceEntry occurence in occurences)
        {
            occurence.daysPassed += passAmount;
            occurence.UIObject.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
            occurence.daysPassed == 1? "Yesterday" : occurence.daysPassed + " days ago";
        }

        GameManager.Instance.NewOccurence();

        if (_currentDay / 7 > _currentWeek)
        {
            _currentWeek++;
            GameManager.Instance.WeekPassed();
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
