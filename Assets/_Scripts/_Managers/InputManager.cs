using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// InputManager: inizializza il nuovo Input System (InputSystem_Actions)
/// e fornisce helper/abstraction per i vari input usati nel gioco (solo mouse + tastiera).
/// Singleton semplice pensato come punto unico di accesso per gli altri sistemi.
/// </summary>
public class InputManager : MonoBehaviour
{
    // 1) Statici / costanti
    private static readonly string DEFAULT_CURSOR_NAME = "DefaultCursor";

    // 2) Campi pubblici/serializzati
    [SerializeField] private bool lockCursorOnStart = true;

    // 3) Campi privati
    private InputSystem_Actions inputActions;
    private static InputManager _instance;

    // 4) Proprietà
    public static InputManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    public InputSystem_Actions Actions
    {
        get { return inputActions; }
    }

    // Event opzionale: chiamato ogni volta che arriva un valore di look (delta mouse).
    public event Action<Vector2> OnLookPerformed = delegate { };

    // 5) MonoBehaviour methods (ordine consigliato)
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeInputSystem();
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Enable();
        }

        if (lockCursorOnStart)
        {
            SetCursorLocked(true);
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            // rimuoviamo le subscription in modo sicuro
            inputActions.Player.Look.performed -= OnLookPerformedCallback;
            inputActions.Player.Look.canceled -= OnLookCanceledCallback;

            inputActions.Disable();
            inputActions.Dispose();
            inputActions = null;
        }
    }

    // 6) Metodi pubblici (helper)
    public Vector2 GetMove()
    {
        if (inputActions == null) return Vector2.zero;
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetLook()
    {
        if (inputActions == null) return Vector2.zero;
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

    public bool IsJumpPressed()
    {
        if (inputActions == null) return false;
        return inputActions.Player.Jump.triggered;
    }

    public bool IsFirePressed()
    {
        if (inputActions == null) return false;
        return inputActions.Player.Fire.IsPressed();
    }

    public bool IsAimPressed()
    {
        if (inputActions == null) return false;
        return inputActions.Player.Aim.IsPressed();
    }

    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    // 7) Metodi privati
    private void InitializeInputSystem()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        // SUBSCRIBE: non fare controllo a "inputActions.Player != null" perché Player è un struct.
        // Controllo solo che inputActions sia stato creato.
        if (inputActions != null)
        {
            inputActions.Player.Look.performed += OnLookPerformedCallback;
            inputActions.Player.Look.canceled += OnLookCanceledCallback;
        }
    }

    private void OnLookPerformedCallback(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        OnLookPerformed.Invoke(v);
    }

    private void OnLookCanceledCallback(InputAction.CallbackContext ctx)
    {
        OnLookPerformed.Invoke(Vector2.zero);
    }
}
