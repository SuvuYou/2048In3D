using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Helpers;

namespace Cube
{
    /// <summary>
    /// Uses UglySerializableDictionary to allow to set a Dictionary in the inspector
    /// 
    /// Has two Dictionaries:
    /// - Colors of cubes for each PO2
    /// - Spawn probabilities for each PO2 
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/Cubes/CubeConfigurations")]
    public class CubeConfigurationsSO : ScriptableObject
    {
        private const int DEFAULT_FALLBACK_PO2_VALUE = 2;

        #region Cube Colors
        [SerializeField] private UglySerializableDictionary<int, Color> CubeColorPerPO2ValueSerializable;

        private Dictionary<int, Color> _cubeColorPerPO2Value;
        public Dictionary<int, Color> CubeColorPerPO2Value 
        {
            get
            {
                if (_cubeColorPerPO2Value == null) 
                {
                    _cubeColorPerPO2Value = CubeColorPerPO2ValueSerializable.ToDictionary();
                }

                return _cubeColorPerPO2Value;
            }
        }

        public Color GetColorByPO2Value(int po2Value) 
        {
            if (CubeColorPerPO2Value.TryGetValue(po2Value, out Color color)) 
            {
                return color;
            }

            Debug.LogWarning($"CubeColors has no value for {po2Value}, creating a new one");

            Color newColorAssignment = Random.ColorHSV();
            CubeColorPerPO2Value.Add(po2Value, newColorAssignment);

            return newColorAssignment;
        }
        #endregion

        #region Cube Spawn Probabilities
        [SerializeField] private UglySerializableDictionary<int, float> CubeSpawnProbabilitiesSerializable;

        private Dictionary<int, float> _cubeSpawnProbabilities;
        public Dictionary<int, float> CubeSpawnProbabilities 
        {
            get
            {
                if (_cubeSpawnProbabilities == null) 
                {
                    _cubeSpawnProbabilities = CubeSpawnProbabilitiesSerializable.ToDictionary();
                }

                return _cubeSpawnProbabilities;
            }
        }

        public int GetRandomPo2Value()
        {
            float total = CubeSpawnProbabilities.Values.Sum();

            if (total <= 0f) 
            {
                Debug.LogWarning("CubeSpawnProbabilities has no values, defaulting to DEFAULT_FALLBACK_PO2_VALUE");

                return DEFAULT_FALLBACK_PO2_VALUE;
            }

            float roll = Random.value * total;
            float cumulative = 0f;

            foreach (var kvp in CubeSpawnProbabilities)
            {
                cumulative += kvp.Value;
                
                if (roll <= cumulative) return kvp.Key;
            }

            return CubeSpawnProbabilities.Keys.Last();
        }
        #endregion
    }
}
