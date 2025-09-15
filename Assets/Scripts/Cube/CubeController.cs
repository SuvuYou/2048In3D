using System;
using System.Collections;
using Helpers;
using UnityEngine;

namespace Cube
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeController : MonoBehaviour, IPoolableObject
    {
        public void ResetProperties() 
        {
            _isInteractable = true;
            _collider.excludeLayers = 0;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            SetCubePo2Value(0);

            _view.ResetProperties();
        }

        [Header("References")]
        [SerializeField] private CubeConfigurationsSO _cubeConfigurations;
        [SerializeField] private GlobalEventsBusSO _globalsEventsBus;
        [SerializeField] private CubeView _view;

        [Header("Collision")]
        [SerializeField] private Collider _collider;
        [SerializeField] private LayerMask _excludeLayersOnDestroy;

        [Header("Launch")]
        [SerializeField] private float _launchForce;

        public int PO2Value { get; private set; }
        public float CurrentVelocity => _rigidbody.velocity.magnitude;

        private Rigidbody _rigidbody;
        private bool _isInteractable;

        private Action _onCubeDestroy;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _isInteractable = true;
        }

        private void Start() 
        {
            _globalsEventsBus.OnGameEnd += DestroySelf;
        }

        private void OnDestroy() 
        {
            _globalsEventsBus.OnGameEnd -= DestroySelf;
        }

        public void Init(Action onCubeDestroy)
        {
            _onCubeDestroy = onCubeDestroy;
        }

        public void SetCubePo2Value(int po2Value)
        {
            PO2Value = po2Value;

            _view.SetText(PO2Value.ToString());
            _view.SetCubeColor(_cubeConfigurations.GetColorByPO2Value(PO2Value));
        }

        public void Launch(Vector3 direction)
        {
            _rigidbody.AddForce(direction * _launchForce, ForceMode.Impulse);

            _globalsEventsBus.TriggerOnCubeLaunch(transform.position);
        }

        public void SlideToPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void DestroySelf() 
        {
            if (!_isInteractable) return;

            _collider.excludeLayers = _excludeLayersOnDestroy;
            _isInteractable = false;
            StartCoroutine(DestroySequence());
        } 

        private IEnumerator DestroySequence()
        {
            Coroutine shrinkCoroutine = _view.TryShrinkCube();

            yield return shrinkCoroutine;

            _onCubeDestroy?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isInteractable) return;

            if (collision.gameObject.TryGetComponent(out CubeController otherCube))
            {
                if (MergeSystem.TryMerge(this, otherCube, collision, out int score))
                {
                    _view.TryPopUpCube();
                    _globalsEventsBus.TriggerOnCubesMerged(transform.position, score);
                }
            }

            if (collision.gameObject.TryGetComponent(out GameEndCollider endNet))
            {
                _globalsEventsBus.TriggerOnGameEnd();
            }
        }
    }
}
