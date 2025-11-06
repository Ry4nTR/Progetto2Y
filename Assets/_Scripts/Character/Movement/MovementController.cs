using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputHandler playerInputHandler;

    private bool myJump;

    private void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController> ();

        if (playerInputHandler == null)
            playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    void Jump()
    {
        characterController.Move(new Vector3(0, 1, 0));
    }

    private void FixedUpdate()
    {
        myJump = playerInputHandler.jump;

        if (myJump)
            Jump();
    }
}
