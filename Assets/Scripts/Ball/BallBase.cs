using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ball
{
    public abstract class BallBase : MonoBehaviour
    {
        private float _xStep;
        private float _yStep;
        [SerializeField] private float m_launchRange = 1.0f;
        [SerializeField] private float m_speed = 5f;
        [SerializeField] private float m_initialSpeedMultiplier = 2f/3;

        void Awake()
        {
            _xStep = Random.Range(-1, 1) >= 0 ? m_speed : -m_speed;
            _xStep *= m_initialSpeedMultiplier;
            _yStep = Random.Range(-m_launchRange, m_launchRange);
        }


        void Update()
        {
            _yStep = Mathf.Clamp(_yStep, -10, 10);
            transform.Translate(_xStep * Time.deltaTime, _yStep * Time.deltaTime, 0);
        }
        

        protected abstract bool CheckPlayerCollision(Player player);

        protected bool BounceBack(Player player)
        {
            if (player.PlayerType == PlayerType.Player1)
            {
                return transform.position.x > player.transform.position.x;
            }

            if (player.PlayerType == PlayerType.Player2)
            {
                return transform.position.x < player.transform.position.x;
            }

            return false;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject go = other.gameObject;
            if (go.CompareTag("Player"))
            {
                var player = go.GetComponent<Player>();
                if (!CheckPlayerCollision(player)) return;
                if (BounceBack(player))
                {
                    _xStep = Mathf.Abs(_xStep) < m_speed ? _xStep < 0 ? m_speed : -m_speed : -_xStep;
                }
                else
                {
                    _yStep = -_yStep;
                }
                _yStep += player.Rigidbody.velocity.y;

            }
            else if (go.CompareTag("Boundary"))
            {
                if (go.name.Equals("UpperBound"))
                {
                    _yStep = _yStep > 0 ? -_yStep : _yStep;
                }
                else
                {
                    _yStep = _yStep < 0 ? -_yStep : _yStep;
                }
            }
        }

        //FailSafe for too fast movement
        private void OnTriggerStay2D(Collider2D other)
        {
            
            GameObject go = other.gameObject;
            if (go.CompareTag("Boundary"))
            {
                if (go.name.Equals("UpperBound"))
                {
                    _yStep = _yStep > 0 ? -_yStep : _yStep;
                }
                else
                {
                    _yStep = _yStep < 0 ? -_yStep : _yStep;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("BottomLine"))
            {
                GameManager.Instance.Goal(other.name == "LeftGoal" ? PlayerType.Player1 : PlayerType.Player2, 1);
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }

}
