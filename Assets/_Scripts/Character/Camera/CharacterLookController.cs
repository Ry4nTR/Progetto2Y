using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLookController : MonoBehaviour
{
    // 1) Statici / costanti
    private const float DEFAULT_MAX_PITCH = 50;
    private const float DEFAULT_MIN_PITCH = -40f;

    // 2) Campi pubblici/serializzati
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float sensitivity = .1f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private float maxPitch = DEFAULT_MAX_PITCH;
    [SerializeField] private float minPitch = DEFAULT_MIN_PITCH;

    // 3) Campi privati
    private float pitch;
    private float yaw;

    // 4) Proprietà
    public float Pitch
    {
        get { return pitch; }
        private set { pitch = Mathf.Clamp(value, minPitch, maxPitch); }
    }

    public float Yaw
    {
        get { return yaw; }
        private set { yaw = value; }
    }

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = Mathf.Max(0.01f, value); }
    }

    // 5) MonoBehaviour methods
    private void Awake()
    {
        Vector3 e = transform.eulerAngles;
        Yaw = e.y;

        if (cameraHolder != null)
        {
            float localPitch = cameraHolder.localEulerAngles.x;
            if (localPitch > 180f) localPitch -= 360f;
            Pitch = localPitch;
        }
        else
        {
            Pitch = 0f;
        }
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SetCursorLocked(true);
        }
    }

    private void Start()
    {
        if (cameraHolder == null)
        {
            Debug.LogWarning("CharacterLookController: cameraHolder non è assegnato. Pitch non applicato.", this);
        }
    }

    private void Update()
    {
        HandleLook();
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SetCursorLocked(false);
        }
    }

    // 6) Metodi pubblici
    public void ResetView()
    {
        Yaw = 0f;
        Pitch = 0f;
        ApplyRotationInstant();
    }

    public void SetView(Vector2 yawPitch)
    {
        Yaw = yawPitch.x;
        Pitch = yawPitch.y;
        ApplyRotationInstant();
    }

    public void SetSensitivity(float newSensitivity)
    {
        Sensitivity = newSensitivity;
    }

    // 7) Metodi privati
    private void HandleLook()
    {
        if (InputManager.Instance == null) return;

        // raw delta in pixel/frame (o come è configurato nelle action)
        Vector2 rawDelta = InputManager.Instance.GetLook();

        // invert Y se richiesto
        if (invertY) rawDelta.y = -rawDelta.y;

        // applica solo sensitivity: rawDelta * sensitivity
        // NON uso Time.deltaTime né smoothing per massima reattività
        float deltaYaw = rawDelta.x * Sensitivity;
        float deltaPitch = rawDelta.y * Sensitivity;

        Yaw += deltaYaw;
        Pitch += deltaPitch;
        Pitch = Mathf.Clamp(Pitch, minPitch, maxPitch);

        // Applica rotazioni immediate
        transform.rotation = Quaternion.Euler(0f, Yaw, 0f);

        if (cameraHolder != null)
        {
            cameraHolder.localRotation = Quaternion.Euler(-Pitch, 0f, 0f);
        }
    }

    private void ApplyRotationInstant()
    {
        transform.rotation = Quaternion.Euler(0f, Yaw, 0f);

        if (cameraHolder != null)
        {
            cameraHolder.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
        }
    }
}