using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private InputSystem_Actions inputActions;

    public event Action OnLeftClick;
    public event Action OnRightClick;
    public event Action OnEscapePressed;

    public Vector2 MousePosition { get; private set; }
    public Vector2 MouseDelta { get; private set; }

    private Vector2 lastMousePosition;

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
            return;
        }

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        if (inputActions == null) return;

        inputActions.Enable();

        inputActions.UI.Click.performed += HandleLeftClick;
        inputActions.UI.RightClick.performed += HandleRightClick;
        inputActions.UI.Cancel.performed += HandleEscape;
    }

    private void OnDisable()
    {
        if (inputActions == null) return;

        inputActions.UI.Click.performed -= HandleLeftClick;
        inputActions.UI.RightClick.performed -= HandleRightClick;
        inputActions.UI.Cancel.performed -= HandleEscape;

        inputActions.Disable();
    }

    private void Update()
    {
        MousePosition = inputActions.UI.Point.ReadValue<Vector2>();

        MouseDelta = MousePosition - lastMousePosition;
        lastMousePosition = MousePosition;
    }

    private void HandleLeftClick(InputAction.CallbackContext ctx)
    {
        OnLeftClick?.Invoke();
    }

    private void HandleRightClick(InputAction.CallbackContext ctx)
    {
        OnRightClick?.Invoke();
    }

    private void HandleEscape(InputAction.CallbackContext ctx)
    {
        OnEscapePressed?.Invoke();
    }
}