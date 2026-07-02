using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions inputActions;

    // Referencias directas a las acciones
    private InputAction moveAction;
    private InputAction jumpAction;

    private bool inputEnabled = true;

    #region Movement

    public Vector2 MoveInput { get; private set; }

    public float MoveX => MoveInput.x;

    public float MoveY => MoveInput.y;

    #endregion

    #region Jump

    public bool JumpPressed { get; private set; }

    public bool JumpHeld { get; private set; }

    public bool JumpReleased { get; private set; }

    #endregion

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;
        jumpAction = inputActions.Player.Jump;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            ResetInputState();
            return;
        }

        MoveInput = moveAction.ReadValue<Vector2>();

        JumpPressed = jumpAction.WasPressedThisFrame();
        JumpHeld = jumpAction.IsPressed();
        JumpReleased = jumpAction.WasReleasedThisFrame();

#if UNITY_EDITOR
        DebugInputs();
#endif
    }

    /// <summary>
    /// Activa o desactiva el control del jugador.
    /// </summary>
    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;

        if (!enabled)
        {
            ResetInputState();
        }
    }

    /// <summary>
    /// Reinicia todos los estados de entrada.
    /// </summary>
    private void ResetInputState()
    {
        MoveInput = Vector2.zero;

        JumpPressed = false;
        JumpHeld = false;
        JumpReleased = false;
    }

#if UNITY_EDITOR

    private void DebugInputs()
    {
        if (JumpPressed)
        {
            Debug.Log("Jump");
        }

        if (Mathf.Abs(MoveX) > 0.01f)
        {
            Debug.Log($"Move X: {MoveX}");
        }

        if (Mathf.Abs(MoveY) > 0.01f)
        {
            Debug.Log($"Move Y: {MoveY}");
        }
    }

#endif
}