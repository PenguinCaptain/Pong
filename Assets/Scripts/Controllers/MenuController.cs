using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class MenuController : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 1;
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene($"Scenes/{sceneName}");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
