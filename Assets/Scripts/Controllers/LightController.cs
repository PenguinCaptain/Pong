using Managers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Controllers
{
    public class LightController : SingletonMonoBehavior<LightController>
    {
        private Light2D _light2D;
        [SerializeField] private int m_scoreToDark;
        public float CurrentIntensity => _light2D.intensity;
        void Start()
        {
            _light2D = GetComponent<Light2D>();
            _light2D.intensity = 1.0f;
            GameManager.Instance.OnScore += UpdateLight;
        }

        private void UpdateLight(GameManager game)
        {
            float currentMax = Mathf.Max(game.Score1, game.Score2);
            _light2D.intensity = Mathf.Lerp(1, 0, currentMax / m_scoreToDark);
        }

    }
}
