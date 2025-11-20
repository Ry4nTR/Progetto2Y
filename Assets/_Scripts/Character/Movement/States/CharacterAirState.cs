using UnityEngine;

namespace SyncedRush.Character.Movement
{
	public class CharacterAirState : CharacterMovementState
    {
        public CharacterAirState(MovementController movementComponentReference) : base(movementComponentReference)
        {
        }

        public override MovementState ProcessFixedUpdate()
        {
            base.ProcessFixedUpdate();

            if (CheckGround())
                return MovementState.Move;

            Fall();
            ProcessMovement();
            return MovementState.None;
        }

        public override void EnterState()
        {
            base.EnterState();

            if (character.State == MovementState.Jump)
                Jump();
        }

        private bool CheckGround()
        {
            if (character.Controller.isGrounded && character.VerticalVelocity <= 0f)
            {
                character.VerticalVelocity = -.1f;
                return true;
            }
            else
                return false;
        }

        private void Jump()
        {
            character.VerticalVelocity += character.Stats.JumpHeight;
        }

        private void Fall()
        {
            character.VerticalVelocity -= (character.Stats.Gravity * Time.fixedDeltaTime);
        }

    }
}