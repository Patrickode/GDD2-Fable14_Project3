using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private Animator resultsAnimator = null;
    [SerializeField] private TextMeshProUGUI currentScoreLabel = null;
    [SerializeField] private TextMeshProUGUI highScoreLabel = null;
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private Color newHighScoreColor = Color.yellow;

    private void Start()
    {
        if (!resultsAnimator) { scoreManager = FindObjectOfType<ScoreManager>(); }

        CustomerManager.LastCustomerDequeued += ShowResultsScreen;
    }
    private void OnDestroy()
    {
        CustomerManager.LastCustomerDequeued -= ShowResultsScreen;
    }

    private void ShowResultsScreen()
    {
        float cScore = scoreManager.CurrentScore;
        float hScore = scoreManager.HighScore;
        currentScoreLabel.text = $"Today's Earnings: ${cScore:F2}";
        highScoreLabel.text = $"Today's Earnings: ${hScore:F2}";

        if (cScore >= hScore)
        {
            currentScoreLabel.color = newHighScoreColor;
            highScoreLabel.color = newHighScoreColor;
        }

        gameObject.SetActive(true);
        resultsAnimator.SetTrigger("In");
    }
}
