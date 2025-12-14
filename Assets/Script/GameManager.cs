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
        
    }
}
