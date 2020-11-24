using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float currentScore;
    public float CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            OnScoreChange?.Invoke(value);
        }
    }

    private string highScoreKey = "hs";
    public float HighScore
    {
        get => PlayerPrefs.GetFloat(highScoreKey, 0);
        set => PlayerPrefs.SetFloat(highScoreKey, value);
    }

    public static Action<float> OnScoreChange;

    private void Start()
    {
        ResetScore();
    }

    private void OnEnable()
    {
        CustomerManager.LastCustomerDequeued += UpdateHighScore;
    }

    private void OnDisable()
    {
        CustomerManager.LastCustomerDequeued -= UpdateHighScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void IncreaseScore(Potion potion)
    {
        float increaseAmount = potion.PotionType.score;
        if (potion.cookState == CookState.Perfect)
        {
            increaseAmount += 20.50f;
        }
        else if (potion.cookState == CookState.Overcooked)
        {
            increaseAmount -= 15.25f;
        }
        CurrentScore += increaseAmount;
    }
    public void DecreaseScore(PotionType potionType)
    {
        CurrentScore -= potionType.score + 100.01f;
    }

    private void UpdateHighScore()
    {
        HighScore = Mathf.Max(CurrentScore, HighScore);
    }
}
