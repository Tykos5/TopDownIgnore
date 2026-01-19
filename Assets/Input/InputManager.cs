using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static Vector2 Movement;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _dashAction;
    public static bool DashPressed;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];

        _dashAction = _playerInput.actions["Dash"];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        DashPressed = _dashAction.WasPerformedThisFrame();
        if (DashPressed)
        {
            Debug.Log("Dash pressed");
        }
    }
}
