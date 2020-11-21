using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            OnScoreChange?.Invoke(value);
        }
    }

    public event Action<int> OnScoreChange;

    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void IncreaseScore(Potion potion)
    {
        CurrentScore += potion.PotionType.score;
    }
}
