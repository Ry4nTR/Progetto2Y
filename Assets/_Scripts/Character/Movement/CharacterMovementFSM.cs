using UnityEngine;
using SyncedRush.Generics;

namespace SyncedRush.Character.Movement
{
	public enum MovementState
	{
		None = 0,
		Move = 1,
		Jump = 2,
	}
	public class CharacterMovementFSM : BaseStateMachine<MovementState, CharacterMovementState>
	{
		// Campi statici/costanti

		// Campi pubblici/serializzati

		// Campi privati

		// Proprietà

		// Metodi MonoBehaviour di Unity
		// 	Awake

		// 	OnEnable

		// 	Start

		// 	Update

		// 	FixedUpdate

		// 	LateUpdate

		// 	OnDisable

		// 	OnDestroy

		// Metodi pubblici

		// Metodi privati

	}
}