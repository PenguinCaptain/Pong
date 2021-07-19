using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class WinController : MonoBehaviour
    {
        [SerializeField] private Text m_winningText;

        public void Win(PlayerType type)
        {
            gameObject.SetActive(true);
            m_winningText.text = "Player "+(type == PlayerType.Player1 ? "1" : "2")+" wins";
        }
    }
}
