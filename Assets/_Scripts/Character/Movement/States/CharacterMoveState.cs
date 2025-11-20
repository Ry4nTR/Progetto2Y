using UnityEngine;

namespace SyncedRush.Character.Movement
{
	public class CharacterMoveState : CharacterMovementState
	{
        public CharacterMoveState(MovementController movementComponentReference) : base(movementComponentReference)
        {
        }

        public override MovementState ProcessFixedUpdate()
        {
            base.ProcessFixedUpdate();

            if (!CheckGround())
                return MovementState.Air;

            Walk();

            if (character.Input.jump)
                return MovementState.Jump;

            ProcessMovement();
            return MovementState.None;
        }

        private bool CheckGround()
        {
            if (character.IsOnGround)
            {
                character.VerticalVelocity = -.1f;
                return true;
            }
            else
                return false;
        }

        private void Walk()
        {
            if (character.Input.move.magnitude > 0f)
            {
                Vector3 motion = character.Orientation.transform.forward * character.Input.move.y
                    + character.Orientation.transform.right * character.Input.move.x;
                motion.y = 0f;
                motion.Normalize();

                character.HorizontalVelocity = new Vector2(motion.x, motion.z) * character.Stats.WalkSpeed;
            }
            else
            {
                character.HorizontalVelocity = Vector2.MoveTowards(character.HorizontalVelocity, Vector2.zero, 2f * Time.fixedDeltaTime);
            }
        }

	}
}