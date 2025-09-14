using TMPro;
using UnityEngine;

public class GameEndCanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private string _scoreFormat = "Score: {0}";

    public void SetScore(int score) => _scoreText.text = string.Format(_scoreFormat, score);
}
