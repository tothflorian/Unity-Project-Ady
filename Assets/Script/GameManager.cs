using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Game Stats

    private float _moneyLevel;
    private float _moralLevel;
    private float _reputationLevel;
    private float _enviromentLevel;

    #endregion

    public void Start()
    {
        _moneyLevel = 50f;
        _moralLevel = 50f;
        _reputationLevel = 50f;
        _enviromentLevel = 50f;
    } 

    public void WeekPassed()
    {
        _moneyLevel += 10 + _reputationLevel / 4;
    }

    public void ChangeResourceLevel(Occurence occurence)
    {
        foreach ((Resource?, short?) res in occurence.Resources)
        {
            switch (res.Item1)
            {
                case Resource.Money:
                    if (_moneyLevel + res.Item2 > 100)
                        _moneyLevel = 100f;
                    else if (_moneyLevel + res.Item2 < 0)
                        GameOver();
                    else
                        _moneyLevel += (int)res.Item2;
                    break;
                case Resource.Moral:
                    if (_moralLevel + res.Item2 > 100)
                        _moralLevel = 100f;
                    else if (_moralLevel + res.Item2 < 0)
                        GameOver();
                    else
                        _moralLevel += (int)res.Item2;
                    break;
                case Resource.Reputation:
                    if (_reputationLevel + res.Item2 > 100)
                        _reputationLevel = 100f;
                    else if (_reputationLevel + res.Item2 < 0)
                        GameOver();
                    else
                        _reputationLevel += (int)res.Item2;
                    break;
                case Resource.Enviroment:
                    if (_enviromentLevel + res.Item2 > 100)
                        _enviromentLevel = 100f;
                    else if (_enviromentLevel + res.Item2 < 0)
                        GameOver();
                    else
                        _enviromentLevel += (int)res.Item2;
                    break;
            }
        }
    }

    private void GameOver()
    {
        // game over screen, vissza a főmenübe.
    }
}
