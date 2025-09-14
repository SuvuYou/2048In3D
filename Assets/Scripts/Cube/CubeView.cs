using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;

namespace Cube
{
    public class CubeView : MonoBehaviour, IPoolableObject
    {
        public void ResetProperties()
        {
            MeshRenderer.material.color = Color.white;
            transform.localScale = Vector3.one;

            if (_popUpCoroutine != null) StopCoroutine(_popUpCoroutine);
            if (_shrinkCoroutine != null) StopCoroutine(_shrinkCoroutine);
            _popUpCoroutine = null;
            _shrinkCoroutine = null;
        }

        [Header("Numbers TMPro")]
        [field: SerializeField] public TextMeshPro FrontNumber { get; private set; }
        [field: SerializeField] public TextMeshPro BackNumber { get; private set; }
        [field: SerializeField] public TextMeshPro LeftNumber { get; private set; }
        [field: SerializeField] public TextMeshPro RightNumber { get; private set; }
        [field: SerializeField] public TextMeshPro TopNumber { get; private set; }
        [field: SerializeField] public TextMeshPro BottomNumber { get; private set; }
        
        [Header("Material")]
        [field: SerializeField] public Material CubeMaterialBlueprint { get; private set; }
        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

        [Header("Animation")]
        [SerializeField] private float _shrinkDuration;
        [SerializeField] private float _popUpDuration;
        [SerializeField] private float _popUpScaleIncrease;

        private Coroutine _popUpCoroutine;
        private Coroutine _shrinkCoroutine;

        private void Awake()
        {
            var newMaterial = new Material(CubeMaterialBlueprint)
            {
                color = Color.white
            };

            MeshRenderer.material = newMaterial;
        }

        #region Cube Visuals
        public void SetText(string text)
        {
            BackNumber.text = text;
            FrontNumber.text = text;
            TopNumber.text = text;
            BottomNumber.text = text;
            LeftNumber.text = text;
            RightNumber.text = text;
        }

        public void SetCubeColor(Color color)
        {
            MeshRenderer.material.color = color;
        }
        #endregion

        #region Cube Animation
        public Coroutine TryShrinkCube()
        {
            if (_shrinkCoroutine != null) return null;

            if (_popUpCoroutine != null)
            {
                StopCoroutine(_popUpCoroutine);
                _popUpCoroutine = null;
            }

            _shrinkCoroutine = StartCoroutine(ShrinkCoroutine());

            return _shrinkCoroutine;
        }

        public Coroutine TryPopUpCube()
        {
            if (_popUpCoroutine != null || _shrinkCoroutine != null) return null;

            _popUpCoroutine = StartCoroutine(PopUpCoroutine());

            return _popUpCoroutine;
        }

        private IEnumerator ShrinkCoroutine()
        {
            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.zero;

            float elapsed = 0f;

            while (elapsed < _shrinkDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _shrinkDuration;
                transform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            transform.localScale = endScale;
            _shrinkCoroutine = null;
        }

        private IEnumerator PopUpCoroutine()
        {
            Vector3 startScale = transform.localScale;
            Vector3 endScale = startScale + startScale * _popUpScaleIncrease;

            float halfDuration = _popUpDuration / 2f;
            float elapsed = 0f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                transform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            elapsed = 0f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                transform.localScale = Vector3.Lerp(endScale, startScale, t);
                yield return null;
            }

            transform.localScale = startScale;
            _popUpCoroutine = null;
        }
        #endregion
    }
}
