using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class MovementController : MonoBehaviour
{

    [SerializeField] private GameObject _orientation;

    private CharacterController characterController;
    private PlayerInputHandler playerInputHandler;

    /// <summary>
    /// Il vettore di movimento orizzontale del character. Indica lo spostamento orizzontale ad ogni frame del FixedUpdate
    /// </summary>
    private Vector2 _horizontalVelocity;
    /// <summary>
    /// Valore di movimento verticale del character. Indica lo spostamento verticale ad ogni frame del FixedUpdate
    /// </summary>
    private float _verticalVelocity;

    private bool myJump;

    private void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (playerInputHandler == null)
            playerInputHandler = GetComponent<PlayerInputHandler>();
    }


    private void FixedUpdate()
    {
        myJump = playerInputHandler.jump;

        Walk();
        ApplyGravity();
        Jump();

        ProcessMovement();
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && _verticalVelocity <= 0f)
        {
            _verticalVelocity = -.1f;
        }

        _verticalVelocity += (-9.81f * Time.fixedDeltaTime);
    }

    private void Walk()
    {
        if (playerInputHandler.move.magnitude > 0f)
        {
            Vector3 motion = _orientation.transform.forward * playerInputHandler.move.y + _orientation.transform.right * playerInputHandler.move.x;
            motion.y = 0f;
            motion.Normalize();

            //_horizontalVelocity += motion;
            _horizontalVelocity = new Vector2(motion.x, motion.z);
        }
        else
        {
            _horizontalVelocity = Vector2.MoveTowards(_horizontalVelocity, Vector2.zero, 2f * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (myJump && characterController.isGrounded)
        {
            _verticalVelocity += 10f;
        }
    }

    private void ProcessMovement()
    {
        Vector3 _velocity = new Vector3(_horizontalVelocity.x, _verticalVelocity, _horizontalVelocity.y);
        characterController.Move(_velocity * Time.fixedDeltaTime);
    }
}
