using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private GlobalEventsBusSO _globalsEventsBus;

    public int CurrentScore { get; private set; }

    private void Start() 
    {
        Display();

        _globalsEventsBus.OnGameEnd += Hide;
        _globalsEventsBus.OnRestartGame += RestartGame;
        _globalsEventsBus.OnCubesMerged += SetScore;

    }

    private void OnDestroy() 
    {
        _globalsEventsBus.OnGameEnd -= Hide;
        _globalsEventsBus.OnRestartGame -= RestartGame;
        _globalsEventsBus.OnCubesMerged -= SetScore;
    }

    public void Hide() => _scoreView.gameObject.SetActive(false);
    public void Display() => _scoreView.gameObject.SetActive(true);

    public void SetScore(Vector3 _, int score) 
    {
        CurrentScore += score;
        _scoreView.SetScore(CurrentScore);
    }

    private void RestartGame()
    {
        CurrentScore = 0;
        _scoreView.SetScore(CurrentScore);
        Display();
    }
}
