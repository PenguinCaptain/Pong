using System;
using Ball;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// ReSharper disable MemberCanBePrivate.Global

namespace Managers
{
    public class GameManager : SingletonMonoBehavior<GameManager>
    {
        public event GameEvent OnScore;

        [SerializeField] private int m_winningScore = 10;
        [SerializeField] private GameObject m_pausePanel;
        [SerializeField] private WinController m_winController;

        public int WinningScore => m_winningScore;
        public int Score1 { get; private set; }
        public int Score2 { get; private set; }
        
        public int CurrentRound { get; private set; }

        private BallBase Ball { get; set; }

        public bool IsPlaying { get; private set; }
    
        public bool IsStarted { get; set; }
    
        public bool IsFinished { get; set; }

        public void Goal(PlayerType type,int points)
        {
            if (type == PlayerType.Player1)
            {
                Score1 += points;
            }
            else
            {
                Score2 += points;
            }

            ++CurrentRound;
            OnScore?.Invoke(this);
            ScreenBoundary.Instance.EmitLight(type);
            if (CheckWin()) return;
            Invoke(nameof(RefreshBall),ScreenBoundary.Instance.LightTime);
        }

        private bool CheckWin()
        {
            if (Score1 >= WinningScore)
            {
                Win(PlayerType.Player1);
                return true;
            }

            if (Score2>=WinningScore)
            {
                Win(PlayerType.Player2);
                return true;
            }
            return false;
        }

        public void Win(PlayerType winner)
        {
            m_winController.Win(winner);
            Invoke(nameof(ExitGame),1);
        }
        

        private void RefreshBall()
        {
            if (Ball!=null)
            {
                Destroy(Ball.gameObject);
            }

            if (CurrentRound == 0)
            {
                Ball = BallGenerator.Instance.Generate<NormalBall>();
            }
            else
            {
                int type = Random.Range(0, 3);
                switch (type)
                {
                    case 0: Ball = BallGenerator.Instance.Generate<NormalBall>(); break;
                    case 1: Ball = BallGenerator.Instance.Generate<DynamicBall>(); break;
                    case 2: Ball = BallGenerator.Instance.Generate<StaticBall>(); break;
                }
            }
        }

        private void Initialize()
        {
            IsStarted = true;
            Resume();
            RefreshBall();
        }

        void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPlaying)
                    Pause();
                else
                    Resume();
            }
        }

        public void Pause()
        {
            if (!IsPlaying) return;
            m_pausePanel.SetActive(true);
            Time.timeScale = 0;
            IsPlaying = false;
        }

        public void Resume()
        {
            if(IsPlaying) return;
            m_pausePanel.SetActive(false);
            Time.timeScale = 1;
            IsPlaying = true;
        }

        public void ExitGame()
        {
            IsFinished = true;
            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }

    public delegate void GameEvent(GameManager game);

    public enum PlayerType
    {
        Player1,Player2
    }
}