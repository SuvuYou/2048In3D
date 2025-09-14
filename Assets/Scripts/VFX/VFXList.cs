using UnityEngine;

[CreateAssetMenu(fileName = "VFXList", menuName = "ScriptableObjects/VFX/VFXList")]
public class VFXListSO : ScriptableObject
{
    public AudioClip[] Launch; 
    public AudioClip[] Merge; 
}
