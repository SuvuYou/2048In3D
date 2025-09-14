using UnityEngine;

public class GameEndCanvasController : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private GameEndCanvasView _gameEndView;

    [SerializeField] private GlobalEventsBusSO _globalEventsBus;

    private void Start() 
    {
        Hide();
        
        _globalEventsBus.OnGameEnd += Display;
        _globalEventsBus.OnRestartGame += Hide;
    }

    private void OnDestroy() 
    {
        _globalEventsBus.OnGameEnd -= Display;
        _globalEventsBus.OnRestartGame -= Hide;
    }

    public void Display() 
    {
        _scoreController.Hide();
        _gameEndView.SetScore(_scoreController.CurrentScore);

        _gameEndView.gameObject.SetActive(true);
    }

    public void Hide() 
    {
        _scoreController.Display();
        _gameEndView.gameObject.SetActive(false);
    }
}
