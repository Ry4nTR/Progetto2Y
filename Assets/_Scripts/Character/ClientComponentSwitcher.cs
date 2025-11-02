using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Attiva/disattiva componenti all'interno del player controllando se il player è l'owner.
/// <!--/summary>-->
public class ClientComponentSwitcher : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private CharacterLookController lookController;

    private void Awake()
    {
        playerInput.enabled = false;
        inputHandler.enabled = false;
        lookController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerInput.enabled = true;
            inputHandler.enabled = true;
        }

        if (IsServer)
        {
            lookController.enabled = true;
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint, bool fire, bool aim)
    {
        inputHandler.MoveInput(move);
        inputHandler.LookInput(look);
        inputHandler.JumpInput(jump);
        inputHandler.SprintInput(sprint);
        inputHandler.FireInput(fire);
        inputHandler.AimInput(aim);
    }

    private void LateUpdate()
    {
        if(!IsOwner)
            return;

        UpdateInputServerRpc(inputHandler.move, inputHandler.look, inputHandler.jump, inputHandler.sprint, inputHandler.fire, inputHandler.aim);
    }

}
