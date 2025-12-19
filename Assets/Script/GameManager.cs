using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region Game Stats

    private short _moneyLevel;
    private short _moralLevel;
    private short _reputationLevel;
    private short _environmentLevel;
    public short Money {get {return _moneyLevel;}}
    public short Moral {get{return _moralLevel;}}
    public short Reputation {get{return _reputationLevel;}}
    public short Environment {get{return _environmentLevel;}}

    private GridLayoutGroup statParent;
    private RectTransform _money, _moral, _reputation, _environment;
    public Occurence _currentOccurence;
    private System.Random _random;
    public bool newGame = true;

    #endregion
    public static GameManager Instance { get; private set;}

    public Occurence CurrentOccurence {
        get { return _currentOccurence; }
        set { _currentOccurence = value; }
    }
    public Button solveButton;
    public Button ignoreButton;
    [SerializeField]public OccurenceManager _occurenceManager;
    public GameObject occurenceListGameObject;
    public GameObject occurencePrefab;
    public List<OccurenceEntry> occurenceList;
    public Sprite moneyIcon, heatingIcon, politicsIcon, environmentIcon, wifiIcon, waterIcon, electricityIcon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        occurenceList = new List<OccurenceEntry>();

        _moneyLevel = 50;
        _moralLevel = 50;
        _reputationLevel = 50;
        _environmentLevel = 50;

        _random = new System.Random();
    } 
    private void InitData(string path, out int? savedDays)
    {
        occurenceList = new List<OccurenceEntry>();
        using (StreamReader sr = new StreamReader(path))
        {
            savedDays = Convert.ToInt32(sr.ReadLine().Trim());
            
            string[] stats = sr.ReadLine().Split();
            _moneyLevel = Convert.ToInt16(stats[0]);
            _moralLevel = Convert.ToInt16(stats[1]);
            _reputationLevel = Convert.ToInt16(stats[2]);
            _environmentLevel = Convert.ToInt16(stats[3]);

            string line;
            while((line = sr.ReadLine()) != null)
            {
                string[] split = line.Split('|');

                Response res = split[1] == "Solve"? Response.Solve : Response.Ignore;

                Sprite temp;
                switch (split[3])
                {
                    case "money":
                        temp = moneyIcon;
                    break;
                    case "environment":
                        temp = environmentIcon;
                    break;
                    case "water":
                        temp = waterIcon;
                    break;
                    case "electricity":
                        temp = electricityIcon;
                    break;
                    case "politics":
                        temp = politicsIcon;
                    break;
                    case "heating":
                        temp = heatingIcon;
                    break;
                    case "wifi":
                        temp = wifiIcon;
                    break;
                    default:
                        throw new ArgumentException();
                }
                
                Occurence currentOccurence = new Occurence(split[3], temp, split[0], res);

                AddOccurence(currentOccurence,Convert.ToInt32(split[2]));
            }
        }
    }
    public void BindUI(string path = null)
    {
        if (path == "new")
        {
            ResetToDefault();
            return;
        }
        OccurenceManager.Instance.BindUI();
        StatChangeAnimationScript.Instance.BindUI();

        occurenceListGameObject = GameObject.Find("List");

        solveButton = GameObject.Find("Solve").GetComponent<Button>();
        ignoreButton = GameObject.Find("Ignore").GetComponent<Button>();

        _money = GameObject.Find("Money").transform.GetChild(1).GetComponent<RectTransform>();
        _moral = GameObject.Find("Moral").transform.GetChild(1).GetComponent<RectTransform>();
        _reputation = GameObject.Find("Reputation").transform.GetChild(1).GetComponent<RectTransform>();
        _environment = GameObject.Find("Environment").transform.GetChild(1).GetComponent<RectTransform>();
        
        statParent = GameObject.Find("Stats").GetComponent<GridLayoutGroup>();

        solveButton.onClick.AddListener(() => HandleChoice(Response.Solve));
        ignoreButton.onClick.AddListener(() => HandleChoice(Response.Ignore));

        int? savedDays = null;

        if (path != null)
        {
            InitData(path, out savedDays);
            newGame = false;
        }else
        {
            for (int i = 0; i < occurenceList.Count; i++)
            {
                GameObject uiOccurence = Instantiate(occurencePrefab);

                uiOccurence.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = occurenceList[i].Occurence.CurrentSprite;
                uiOccurence.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = occurenceList[i].Occurence.Title;
                uiOccurence.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = occurenceList[i].Occurence.response.ToString();
                uiOccurence.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
                occurenceList[i].daysPassed == 0? "Today" : 
                (occurenceList[i].daysPassed == 1? "Yesterday" : occurenceList[i].daysPassed + " days ago");

                uiOccurence.transform.SetParent(occurenceListGameObject.transform);
                uiOccurence.transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                occurenceList[i].UIObject = uiOccurence;
            }
        }
        TimeManager.Instance.BindUI(savedDays);

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel*1f/100*statParent.cellSize.y);
        _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel*1f/100*statParent.cellSize.y);
        _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel*1f/100*statParent.cellSize.y);
        _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel*1f/100*statParent.cellSize.y);

        GameObject.Find("Occurence").gameObject.SetActive(false);
        if (!newGame)
            GameObject.Find("Tutorial").gameObject.SetActive(false);
    }

    public void WeekPassed()
    {
        _moneyLevel += (short)(15 + _reputationLevel / 8);
        if (_moneyLevel > 100) _moneyLevel = 100;

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel);
    }

    public void HandleChoice(Response response)
    {
        StatChangeAnimationScript.Instance.StatChange(_currentOccurence, response);

        _currentOccurence.response = response;
        AddOccurence(_currentOccurence, 0);
    }
    public void FixSize()
    {
        statParent = GameObject.Find("Stats").GetComponent<GridLayoutGroup>(); 

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel*1f/100*statParent.cellSize.y);
        _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel*1f/100*statParent.cellSize.y);
        _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel*1f/100*statParent.cellSize.y);
        _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel*1f/100*statParent.cellSize.y);
    }
    private void AddOccurence(Occurence occ, int days)
    {
        GameObject occurence = Instantiate(occurencePrefab);
        
        occurence.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = occ.CurrentSprite;
        occurence.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = occ.Title;
        occurence.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = occ.response.ToString();
        
        occurence.transform.SetParent(occurenceListGameObject.transform);
        occurence.transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        
        occurenceList.Add(new OccurenceEntry(occurence, occ, days));
        
        occurence.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
        occurenceList[occurenceList.Count-1].daysPassed == 0? "Today" : 
        (occurenceList[occurenceList.Count-1].daysPassed == 1? "Yesterday" : occurenceList[occurenceList.Count-1].daysPassed + " days ago");
    }

public int?[] ChangeResourceLevel(Occurence occurence, Response response)
{
    bool hasChoice = occurence.Responses.Count == 2;

    List<int?> nums = new List<int?>();
    if (hasChoice)
    {
        foreach (var res in occurence.ResourcesSI)
        {
            short delta = response == Response.Solve ? res.Item2 : res.Item3;
            nums.Add(ApplyResource(res.Item1, delta));
        }
    }
    else
    {
        foreach (var res in occurence.ResourcesOk)
        {
            nums.Add(ApplyResource(res.Item1, res.Item2));
        }
    }
    return nums.ToArray();
}
private int? ApplyResource(Resource resource, short delta)
{
    int? num = null;
    switch (resource)
    {
        case Resource.Money:
            _moneyLevel = Clamp((short)(_moneyLevel + delta));
            _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel*1f/100*statParent.cellSize.y);
            num = 0;
            break;

        case Resource.Moral:
            _moralLevel = Clamp((short)(_moralLevel + delta));
            _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel*1f/100*statParent.cellSize.y);
            num = 1;
            break;

        case Resource.Reputation:
            _reputationLevel = Clamp((short)(_reputationLevel + delta));
            _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel*1f/100*statParent.cellSize.y);
            num = 2;
            break;

        case Resource.Environment:
            _environmentLevel = Clamp((short)(_environmentLevel + delta));
            _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel*1f/100*statParent.cellSize.y);
            num = 3;
            break;
    }
    return num;
}
private short Clamp(short value)
{
    if (value < 0)
    {
        GameOver();
        return 0;
    }

    return (short)Math.Min(value, 100f);
}

    private void ResetToDefault()
    {
        OccurenceManager.Instance.BindUI();
        TimeManager.Instance.BindUI(0);
        StatChangeAnimationScript.Instance.BindUI();

        newGame = true;

        _moneyLevel = 50;
        _moralLevel = 50;
        _reputationLevel = 50;
        _environmentLevel = 50;

        _currentOccurence = null;

        occurenceList = new List<OccurenceEntry>();

        occurenceListGameObject = GameObject.Find("List");

        solveButton = GameObject.Find("Solve").GetComponent<Button>();
        ignoreButton = GameObject.Find("Ignore").GetComponent<Button>();

        _money = GameObject.Find("Money").transform.GetChild(1).GetComponent<RectTransform>();
        _moral = GameObject.Find("Moral").transform.GetChild(1).GetComponent<RectTransform>();
        _reputation = GameObject.Find("Reputation").transform.GetChild(1).GetComponent<RectTransform>();
        _environment = GameObject.Find("Environment").transform.GetChild(1).GetComponent<RectTransform>();

        statParent = GameObject.Find("Stats").GetComponent<GridLayoutGroup>();

        solveButton.onClick.AddListener(() => HandleChoice(Response.Solve));
        ignoreButton.onClick.AddListener(() => HandleChoice(Response.Ignore));

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel*1f/100*statParent.cellSize.y);
        _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel*1f/100*statParent.cellSize.y);
        _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel*1f/100*statParent.cellSize.y);
        _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel*1f/100*statParent.cellSize.y);

        _random = new System.Random();
        
        GameObject.Find("Occurence").gameObject.SetActive(false);
        if (!newGame)
            GameObject.Find("Tutorial").gameObject.SetActive(false);
            
    }


    public void NewOccurence()
    {
        _currentOccurence = _occurenceManager.OccurenceList[_random.Next(1, _occurenceManager.OccurenceList.Count)];

        switch (_currentOccurence.Icon)
        {
            case "money":
                _currentOccurence.CurrentSprite = moneyIcon;
            break;
            case "environment":
                _currentOccurence.CurrentSprite = environmentIcon;
            break;
            case "water":
                _currentOccurence.CurrentSprite = waterIcon;
            break;
            case "electricity":
                _currentOccurence.CurrentSprite = electricityIcon;
            break;
            case "politics":
                _currentOccurence.CurrentSprite = politicsIcon;
            break;
            case "heating":
                _currentOccurence.CurrentSprite = heatingIcon;
            break;
            case "wifi":
                _currentOccurence.CurrentSprite = wifiIcon;
            break;
            default:
                throw new ArgumentException();
        }
        _occurenceManager.occurenceIcon.transform.GetChild(1).GetComponent<Image>().sprite = _currentOccurence.CurrentSprite;
        _occurenceManager.title.GetComponent<TextMeshProUGUI>().text = _currentOccurence.Title;
        _occurenceManager.description.GetComponent<TextMeshProUGUI>().text = _currentOccurence.Description;
        if (_currentOccurence.Responses.Count == 1)
            _occurenceManager.declineButton.SetActive(false);
        else
            _occurenceManager.declineButton.SetActive(true);
    }
    private void GameOver()
    {
        MenuController.Instance.GameOver();
        SavesController.Instance.Delete(SavesController.Instance.selectedGame);
    }
}
