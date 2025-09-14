using System;
using Helpers;
using Inputs;
using UnityEngine;

namespace Cube
{
    public class CubeFactory : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputTransferSO _inputTransferSO;
        [SerializeField] private GlobalEventsBusSO _globalEventsBus;

        [Header("Cube")]
        [SerializeField] private CubeConfigurationsSO _cubeConfigurations;
        [SerializeField] private CubeController _cubePrefab;
        [SerializeField] private int _initialPoolSize = 20;
        
        [Header("Plane")]
        [SerializeField] private GameObject _platform;

        [Header("Spawn Position")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnMinZPosition;
        [SerializeField] private float _spawnMaxZPosition;

        private Plane _platformPlane;

        private CubeController _currentCube;
        private ObjectPool<CubeController> _cubePool;

        private void OnTouch() => _currentCube = SpawnCube();
        private void OnLetGo() { _currentCube.Launch(Vector3.left); _currentCube = null; }

        private void Start()
        {
            _cubePool = new ObjectPool<CubeController>(_cubePrefab, _initialPoolSize, transform);

            _inputTransferSO.Input.OnTouchEvent += OnTouch;
            _inputTransferSO.Input.OnLetGoEvent += OnLetGo;

            _globalEventsBus.OnRestartGame += _cubePool.ResetPool;

            _platformPlane = new Plane(_platform.transform.up, _platform.transform.position);
        }

        private void OnDisable()
        {
            _inputTransferSO.Input.OnTouchEvent -= OnTouch;
            _inputTransferSO.Input.OnLetGoEvent -= OnLetGo;

            _globalEventsBus.OnRestartGame -= _cubePool.ResetPool;
        }

        private void Update()
        {
            if (_currentCube != null)
            {
                Vector3 newCubePosition = GetCubePosition(GetWorldSpacePositionFromTouchPosition());
                _currentCube.SlideToPosition(newCubePosition);
            }
        }

        public CubeController SpawnCube()
        {
            CubeController cube = _cubePool.Get();
            cube.ResetProperties();
            cube.Init(onCubeDestroy: () => _cubePool.ReturnToPool(cube));

            int initialPo2Value = _cubeConfigurations.GetRandomPo2Value();
            cube.SetCubePo2Value(initialPo2Value);

            return cube;
        }

        private Vector3 GetCubePosition(float xPosition)
        {
            return new Vector3(
                _spawnPoint.position.x,
                _spawnPoint.position.y,
                Math.Clamp(xPosition, _spawnMinZPosition, _spawnMaxZPosition)
            );
        }

        private float GetWorldSpacePositionFromTouchPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(_inputTransferSO.Input.TouchPosition);

            if (_platformPlane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter).z;
            }
            
            return 0;
        }
    }
}
