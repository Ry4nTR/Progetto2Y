using SyncedRush.Generics;
using UnityEngine;

namespace SyncedRush.Character.Movement
{
	public abstract class CharacterMovementState : BaseState<MovementState>
	{
		protected MovementController character;

        protected CharacterMovementState(MovementController movementComponentReference)
        {
            character = movementComponentReference;
        }

        protected void ProcessMovement()
        {
            Vector3 _velocity = new Vector3(character.HorizontalVelocity.x, character.VerticalVelocity, character.HorizontalVelocity.y);
            character.Controller.Move(_velocity * Time.fixedDeltaTime);
        }
    }
}