using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class AboutPageController : MonoBehaviour
    {
        [SerializeField] private float m_animDuration;

        private bool _isPopped;
        void Update()
        {
            if (_isPopped && (Input.GetMouseButtonDown(0) ||
                               Input.GetMouseButtonDown(1) ||
                               Input.GetKeyDown(KeyCode.Space) ||
                               Input.GetKeyDown(KeyCode.Escape)))
            {
                CloseDown();
            }
        }

        public void PopUp()
        {
            if (_isPopped)
                return;
            transform.DOScale(1, m_animDuration).onComplete+=()=>_isPopped = true;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void CloseDown()
        {
            if (!_isPopped) return;
            transform.localScale = Vector3.zero;
            _isPopped = false;
        }
    }
}
