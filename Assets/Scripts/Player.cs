using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private KeyCode m_keyUp;
    [SerializeField]private KeyCode m_keyDown;
    [SerializeField]private float m_speed = 10.0f;
    [SerializeField]private PlayerType m_playerType;
    public PlayerType PlayerType => m_playerType;
    private InputManager _inputManager;
    public Rigidbody2D Rigidbody { get; private set; }


    void Start()
    {
        _inputManager = InputManager.Instance;
        Rigidbody = GetComponent<Rigidbody2D>();
        _inputManager.RegisterKey(m_keyUp);
        _inputManager.RegisterKey(m_keyDown);
    }
    
    void Update()
    {
        float mult = _inputManager.GetKey(m_keyUp) - _inputManager.GetKey(m_keyDown);
        Rigidbody.velocity = Vector2.up * (mult * m_speed);
    }

    private void OnDestroy()
    {
        _inputManager.RemoveKey(m_keyUp);
        _inputManager.RemoveKey(m_keyDown);
    }
}
