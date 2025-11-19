using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(CharacterStats))]
public class MovementController : MonoBehaviour
{

    [SerializeField] private GameObject _orientation;

    private PlayerInputHandler _playerInputHandler;
    private CharacterController _characterController;
    private CharacterStats _characterStats;

    /// <summary>
    /// Il vettore di movimento orizzontale del character. Indica lo spostamento orizzontale ad ogni frame del FixedUpdate
    /// </summary>
    private Vector2 _horizontalVelocity;
    /// <summary>
    /// Valore di movimento verticale del character. Indica lo spostamento verticale ad ogni frame del FixedUpdate
    /// </summary>
    private float _verticalVelocity;

    private bool myJump;

    private void Awake()
    {
        if (_playerInputHandler == null)
            _playerInputHandler = GetComponent<PlayerInputHandler>();

        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();

        if (_characterStats == null)
            _characterStats = GetComponent<CharacterStats>();
    }


    private void FixedUpdate()
    {
        myJump = _playerInputHandler.jump;

        Walk();
        ApplyGravity();
        Jump();

        ProcessMovement();
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity <= 0f)
        {
            _verticalVelocity = -.1f;
        }

        _verticalVelocity -= (_characterStats.Gravity * Time.fixedDeltaTime);
    }

    private void Walk()
    {
        if (_playerInputHandler.move.magnitude > 0f)
        {
            Vector3 motion = _orientation.transform.forward * _playerInputHandler.move.y + _orientation.transform.right * _playerInputHandler.move.x;
            motion.y = 0f;
            motion.Normalize();

            //_horizontalVelocity += motion;
            _horizontalVelocity = new Vector2(motion.x, motion.z) * _characterStats.WalkSpeed;
        }
        else
        {
            _horizontalVelocity = Vector2.MoveTowards(_horizontalVelocity, Vector2.zero, 2f * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (myJump && _characterController.isGrounded)
        {
            _verticalVelocity += _characterStats.JumpHeight;
        }
    }

    private void ProcessMovement()
    {
        Vector3 _velocity = new Vector3(_horizontalVelocity.x, _verticalVelocity, _horizontalVelocity.y);
        _characterController.Move(_velocity * Time.fixedDeltaTime);
    }
}
