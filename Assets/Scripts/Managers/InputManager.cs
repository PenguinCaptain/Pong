using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InputManager : SingletonMonoBehavior<InputManager>
    {

        private readonly Dictionary<KeyCode, float> _accumulate = new Dictionary<KeyCode, float>();
        private readonly List<KeyCode> _registered = new List<KeyCode>();

        [SerializeField] private float m_acceleration = 1.0f;
        [SerializeField] private float m_deceleration = 1.0f;
    
        public void RegisterKey(KeyCode key)
        {
            _accumulate[key] = 0;
            UpdateReg();
        }
    
        public void RemoveKey(KeyCode key)
        {
            _accumulate.Remove(key);
            UpdateReg();
        }

        private void UpdateReg()
        {
            _registered.Clear();
            _registered.AddRange(_accumulate.Keys);
        }
    
    
        public float GetKey(KeyCode key)
        {
            if (_accumulate.TryGetValue(key,out var value))
            {
                return value;
            }
            return 0;
        }


        void Update()
        {
            if (!GameManager.Instance.IsPlaying)
            {
                return;
            }
            foreach (var key in _registered)
            {
                if (Input.GetKey(key))
                {
                    _accumulate[key] += Time.deltaTime * m_acceleration; 
                }
                else
                {
                    _accumulate[key] -= Time.deltaTime * m_deceleration;
                }

                _accumulate[key] = Mathf.Clamp01(_accumulate[key]);
            }
        }
    }
}
