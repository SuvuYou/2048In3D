using TMPro;
using UnityEngine;

namespace UI
{
    public class GameEndCanvasView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private string _scoreFormat = "Score: {0}";

        public void SetScore(int score) => _scoreText.text = string.Format(_scoreFormat, score);
    }
}
