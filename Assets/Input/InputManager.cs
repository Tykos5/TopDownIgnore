using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static Vector2 Movement;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private InputAction _dashAction;
    public static bool DashPressed;

    private InputAction _attackAction;
    public static bool AttackPressed;

    private InputAction _spearAction;
    public static bool SpearPressed;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];

        _dashAction = _playerInput.actions["Dash"];

        _attackAction = _playerInput.actions["Attack"];

        _spearAction = _playerInput.actions["Spear"];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        DashPressed = _dashAction.WasPerformedThisFrame();
        //if (DashPressed)
        //{
        //    Debug.Log("Dash pressed");
        //}

        AttackPressed = _attackAction.WasPerformedThisFrame();
        //if (AttackPressed)
        //{
        //    Debug.Log("Attack pressed");
        //}

        SpearPressed = _spearAction.WasPerformedThisFrame();
        if (SpearPressed)
        {
            //Debug.Log("Spear pressed");
        }
    }
}
