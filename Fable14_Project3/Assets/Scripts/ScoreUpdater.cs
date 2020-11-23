using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (!text) { text = GetComponent<TextMeshProUGUI>(); }
        UpdateScoreText(0);
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChange += UpdateScoreText;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChange -= UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        text.text = $"${score:F2}";
    }
}
