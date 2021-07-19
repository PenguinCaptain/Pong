using System;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Ball
{
    public class BallGenerator : SingletonMonoBehavior<BallGenerator>
    {
        [SerializeField] private GameObject m_normalBall;
        [SerializeField] private GameObject m_dynamicBall;
        [SerializeField] private GameObject m_staticBall;
        [SerializeField] private float m_lightDuration;

        public T Generate<T>() where T : BallBase
        {
            Type ballType = typeof(T);
            GameObject ballObj;
            if (ballType == typeof(NormalBall))
            {
                ballObj = Instantiate(m_normalBall, Vector3.zero, new Quaternion());
            }else if (ballType == typeof(DynamicBall))
            {
                ballObj = Instantiate(m_dynamicBall, Vector3.zero, new Quaternion());
            }else if (ballType == typeof(StaticBall))
            {
                ballObj = Instantiate(m_staticBall, Vector3.zero, new Quaternion());
            }
            else
            {
                return null;
            }

            if (ballObj.TryGetComponent<Light2D>(out var l2d))
            {
                DOTween.To(() => LightController.Instance.CurrentIntensity, v => l2d.intensity = v, 1, m_lightDuration);
            }

            return ballObj.GetComponent<T>();
        }
    }    
}