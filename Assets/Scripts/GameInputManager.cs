using UnityEngine;
using System;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }

    public event Action OnAttackPressed;
    public event Action OnJumpPressed;

    public Vector2 MovementInput { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        MovementInput = new Vector2(moveX, moveY).normalized;

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            OnAttackPressed?.Invoke();
        }

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
    }
}