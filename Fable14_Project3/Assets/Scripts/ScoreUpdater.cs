using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private ScoreManager scoreManager;
    private TextMeshProUGUI text;


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnEnable()
    {
        scoreManager.OnScoreChange += UpdateScoreText;
    }

    private void OnDisable()
    {
        scoreManager.OnScoreChange -= UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        text.text = "Score: " + scoreManager.CurrentScore;
    }
}
