using UnityEngine;

namespace VFX
{
    public class VFXManager : MonoBehaviour
    {
        private const float DEFAULT_VOLUME = 0.5f;

        [SerializeField] private GlobalEventsBusSO _globalEvents;
        [SerializeField] private VFXListSO _vfxList;

        private void Start()
        {
            _globalEvents.OnCubeLaunch += PlaySoundOnCubeLaunch;
            _globalEvents.OnCubesMerged += PlaySoundOnCubeMerge;
        }

        private void OnDestroy()
        {
            _globalEvents.OnCubeLaunch -= PlaySoundOnCubeLaunch;
            _globalEvents.OnCubesMerged -= PlaySoundOnCubeMerge;
        }

        private void PlaySoundOnCubeLaunch(Vector3 pos) => _playSoundAtPoint(_vfxList.Launch, pos);
        private void PlaySoundOnCubeMerge(Vector3 pos, int _) => _playSoundAtPoint(_vfxList.Merge, pos);

        private void _playSoundAtPoint(AudioClip[] clips, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length - 1)], position, DEFAULT_VOLUME);
        }
    }
}