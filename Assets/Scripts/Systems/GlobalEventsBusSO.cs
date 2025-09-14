using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEventsBus", menuName = "ScriptableObjects/Events/GlobalEventsBus")]
public class GlobalEventsBusSO : ScriptableObject
{
    public event Action<Vector3, int> OnCubesMerged;
    public event Action<Vector3> OnCubeLaunch;
    public event Action OnGameEnd;
    public event Action OnRestartGame;

    public void TriggerOnGameEnd () => OnGameEnd?.Invoke();
    public void TriggerOnRestartGame () => OnRestartGame?.Invoke();
    public void TriggerOnCubesMerged (Vector3 soundSourcePosition, int score) => OnCubesMerged?.Invoke(soundSourcePosition, score);
    public void TriggerOnCubeLaunch (Vector3 soundSourcePosition) => OnCubeLaunch?.Invoke(soundSourcePosition);
}
