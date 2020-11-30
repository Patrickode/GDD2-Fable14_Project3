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

    public void IncreaseScore(float increaseAmount)
    {
        increaseAmount = SanitizeScore(increaseAmount);
        CurrentScore += increaseAmount;
    }
    public void DecreaseScore(float decreaseAmount)
    {
        decreaseAmount = SanitizeScore(decreaseAmount);
        CurrentScore -= decreaseAmount;
    }

    private float SanitizeScore(float scoreToSanitize) { return Mathf.Round(scoreToSanitize * 100f) / 100f; }

    private void UpdateHighScore()
    {
        HighScore = Mathf.Max(CurrentScore, HighScore);
    }
}
