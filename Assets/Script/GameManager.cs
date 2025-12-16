using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Game Stats

    private float _moneyLevel;
    private float _moralLevel;
    private float _reputationLevel;
    private float _environmentLevel;

    private RectTransform _money, _moral, _reputation, _environment;
    private Occurence _currentOccurence;
    [SerializeField]private OccurenceManager _occurenceManager;
    private System.Random _random;

    #endregion

    public Occurence CurrentOccurence {
        get { return _currentOccurence; }
        set { _currentOccurence = value; }
    }
    public Button solveButton;
    public Button ignoreButton;

    public void Start()
    {
        _moneyLevel = 50f;
        _moralLevel = 50f;
        _reputationLevel = 50f;
        _environmentLevel = 50f;
        
        _money = GameObject.Find("Money").transform.GetChild(1).GetComponent<RectTransform>();
        _moral = GameObject.Find("Moral").transform.GetChild(1).GetComponent<RectTransform>();
        _reputation = GameObject.Find("Reputation").transform.GetChild(1).GetComponent<RectTransform>();
        _environment = GameObject.Find("Environment").transform.GetChild(1).GetComponent<RectTransform>();

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel);

        _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel);

        _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel);

        _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel);

        _random = new System.Random();

        solveButton.onClick.AddListener(() => HandleChoice(Response.Solve));
        ignoreButton.onClick.AddListener(() => HandleChoice(Response.Ignore));
    } 

    public void WeekPassed()
    {
        _moneyLevel += 10 + _reputationLevel / 4;

        _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel);
    }

    public void HandleChoice(Response response)
    {
        ChangeResourceLevel(_currentOccurence, response);
    }

    public void ChangeResourceLevel(Occurence occurence, Response response)
    {
            List<(Resource?, short?)> res = occurence.Resources;

            int index;
            if (response == Response.Ignore)
                index = 1;
            else
                index = 0;

            switch (res[index].Item1)
            {
                case Resource.Money:
                if (_moneyLevel + res[index].Item2 > 100)
                    _moneyLevel = 100f;
                else if (_moneyLevel + res[index].Item2 < 0)
                    GameOver();
                else
                    _moneyLevel += (int)res[index].Item2;
                _money.sizeDelta = new Vector2(_money.sizeDelta.x, _moneyLevel);
                break;
            case Resource.Moral:
                if (_moralLevel + res[index].Item2 > 100)
                    _moralLevel = 100f;
                else if (_moralLevel + res[index].Item2 < 0)
                    GameOver();
                else
                    _moralLevel += (int)res[index].Item2;
                _moral.sizeDelta = new Vector2(_moral.sizeDelta.x, _moralLevel);
                break;
            case Resource.Reputation:
                if (_reputationLevel + res[index].Item2 > 100)
                    _reputationLevel = 100f;
                else if (_reputationLevel + res[index].Item2 < 0)
                    GameOver();
                else
                    _reputationLevel += (int)res[index].Item2;
                _reputation.sizeDelta = new Vector2(_reputation.sizeDelta.x, _reputationLevel);
                break;
            case Resource.Enviroment:
                if (_environmentLevel + res[index].Item2 > 100)
                    _environmentLevel = 100f;
                else if (_environmentLevel + res[index].Item2 < 0)
                    GameOver();
                else
                    _environmentLevel += (int)res[index].Item2;
                _environment.sizeDelta = new Vector2(_environment.sizeDelta.x, _environmentLevel);
                break;
            }
        }
    public void NewOccurence()
    {
        _currentOccurence = _occurenceManager.OccurenceList[_random.Next(1, _occurenceManager.OccurenceList.Count)];
        _occurenceManager.title.GetComponent<TextMeshProUGUI>().text = _currentOccurence.Title;
        _occurenceManager.description.GetComponent<TextMeshProUGUI>().text = _currentOccurence.Description;
        if (_currentOccurence.Responses.Count == 1)
            _occurenceManager.declineButton.SetActive(false);
        else
            _occurenceManager.declineButton.SetActive(true);
    }
    private void GameOver()
    {
        // game over screen, vissza a főmenübe.
    }
}
