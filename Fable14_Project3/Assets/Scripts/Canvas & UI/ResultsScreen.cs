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
    [SerializeField] private float inAnimDelay = 1;

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

        //If current score is negative, put the negative before the dollar sign.
        //We don't need to do this for the high score, it will always be >= 0.
        currentScoreLabel.text = cScore < 0
            ? $"Today's Earnings: -${-cScore:F2}"
            : $"Today's Earnings: ${cScore:F2}";
        highScoreLabel.text = $"Record Earnings: ${hScore:F2}";

        if (cScore >= hScore)
        {
            currentScoreLabel.color = newHighScoreColor;
            highScoreLabel.color = newHighScoreColor;
        }

        StartCoroutine(DelayInAnim(inAnimDelay));
        CustomerManager.LastCustomerDequeued -= ShowResultsScreen;
    }

    private IEnumerator DelayInAnim(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(true);
        resultsAnimator.SetTrigger("In");
    }
}
