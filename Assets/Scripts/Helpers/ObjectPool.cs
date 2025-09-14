using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool = new();
        private int _initialPoolSize;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _initialPoolSize = initialSize;

            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            return Object.Instantiate(_prefab, _parent);
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }

        public void ResetPool()
        {
            foreach (T obj in _pool)
            {
                obj.gameObject.SetActive(false);
            }

            int objectsToClear = _pool.Count - _initialPoolSize;
            
            for (int i = 0; i < objectsToClear; i++)
            {
                T gameObject = _pool.Dequeue();
                GameObject.Destroy(gameObject.gameObject);
            }
        }
    }
}