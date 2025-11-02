using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// PlayerInputHandler:
/// Component da mettere sul player che riceve input via PlayerInput (Send Messages)
/// e rende disponibili helper / valori per gli altri script (es. Player controller, LookController).
/// </summary>
[DisallowMultipleComponent]
public class PlayerInputHandler : MonoBehaviour
{
    // 1) Campi serializzati
    [Header("Cursor")]
    [SerializeField] private bool lockCursorOnEnable = true;

    [Header("Fallback (optional)")]
    [SerializeField] private bool useFallbackInputActions = false;

    // 2) Campi pubblici (imitando StarterAssetsInputs)
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool fire;
    public bool aim;
    public float scroll;

    // 3) Campi privati
    private PlayerInputSystem fallbackActions;

    // 4) Event facoltativi
    public event Action<Vector2> OnLookEvent = delegate { };

    // 5) MonoBehaviour methods
    private void Awake()
    {
        if (useFallbackInputActions)
        {
            InitializeFallbackActions();
        }
    }

    private void OnEnable()
    {
        if (lockCursorOnEnable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // reset iniziale
        move = Vector2.zero;
        look = Vector2.zero;
        jump = false;
        sprint = false;
        fire = false;
        aim = false;
        scroll = 0f;
    }

    private void OnDisable()
    {
        if (useFallbackInputActions && fallbackActions != null)
        {
            fallbackActions.Disable();
        }
    }

    private void OnDestroy()
    {
        if (fallbackActions != null)
        {
            fallbackActions.Player.Look.performed -= OnFallbackLook;
            fallbackActions.Player.Look.canceled -= OnFallbackLookCanceled;
            fallbackActions.Disable();
            fallbackActions.Dispose();
            fallbackActions = null;
        }
    }

    // 6) SendMessage target methods (PlayerInput -> Send Messages)
#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnFire(InputValue value)
    {
        FireInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void OnScroll(InputValue value)
    {
        ScrollInput(value.Get<float>());
    }
#endif

    // 7) Helper publici (altri script leggono questi valori)
    public void MoveInput(Vector2 newMove)
    {
        move = newMove;
    }

    public void LookInput(Vector2 newLook)
    {
        look = newLook;
        OnLookEvent.Invoke(look);
    }

    public void JumpInput(bool newJump)
    {
        jump = newJump;
    }

    public void SprintInput(bool newSprint)
    {
        sprint = newSprint;
    }

    public void FireInput(bool newFire)
    {
        fire = newFire;
    }

    public void AimInput(bool newAim)
    {
        aim = newAim;
    }

    public void ScrollInput(float newScroll)
    {
        scroll = newScroll;
    }

    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    // 8) Fallback input init (opzionale)
    private void InitializeFallbackActions()
    {
        fallbackActions = new PlayerInputSystem();
        fallbackActions.Enable();
        fallbackActions.Player.Look.performed += OnFallbackLook;
        fallbackActions.Player.Look.canceled += OnFallbackLookCanceled;
    }

    private void OnFallbackLook(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        LookInput(v);
    }

    private void OnFallbackLookCanceled(InputAction.CallbackContext ctx)
    {
        LookInput(Vector2.zero);
    }
}
