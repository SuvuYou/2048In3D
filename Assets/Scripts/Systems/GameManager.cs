using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalEventsBusSO _globalEventsBus;

    private void Start() 
    {
        Application.targetFrameRate = 60;
    }

    public void RestartGame() 
    {
        _globalEventsBus.TriggerOnRestartGame();
    }
}