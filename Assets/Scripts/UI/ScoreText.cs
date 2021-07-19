using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private Text m_score1;
        [SerializeField] private Text m_score2;
        [SerializeField] private float m_tweenDuration = 0.5f;

        private int _cachedScore1, _cachedScore2;
        private void Start()
        {
            GameManager.Instance.OnScore += Score;
        }

        private void Score(GameManager game)
        {
            if (_cachedScore1 != game.Score1)
            {
                m_score1.DOCounter(_cachedScore1, game.Score1, m_tweenDuration, false);
            }
            if (_cachedScore2 != game.Score2)
            {
                m_score2.DOCounter(_cachedScore2, game.Score2, m_tweenDuration, false);
            }

            _cachedScore1 = game.Score1;
            _cachedScore2 = game.Score2;
        }
    }
}