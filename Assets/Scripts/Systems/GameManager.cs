using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalEventsBusSO _globalEventsBus;

    public void RestartGame() 
    {
        _globalEventsBus.TriggerOnRestartGame();
    }
}