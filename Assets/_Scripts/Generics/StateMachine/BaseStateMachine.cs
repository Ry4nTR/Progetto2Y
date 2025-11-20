using System;
using System.Collections.Generic;
using UnityEngine;

namespace SyncedRush.Generics
{
    /// <summary>
    /// State Machine generica ed ereditabile. Overkill? Penso proprio di si, ma me ne sono accorto troppo tardi.<br/>
    /// Il valore di default di TStateEnum (cioè l'elemento 0) indica "Nessuno stato", e serve solo a <see cref="QueuedState"/>.
    /// </summary>
    public abstract class BaseStateMachine<TStateEnum, TState> : MonoBehaviour
        where TStateEnum : Enum
        where TState : BaseState<TStateEnum>
    {

        [SerializeField] private TStateEnum _startingState;
        [SerializeField] private Dictionary<TStateEnum, TState> _states = new();

        public TState CurrentState { get; private set; }
        /// <summary> Il QueuedState serve agli stati in uscita per capire qual'è il prossimo stato </summary>
        public TStateEnum QueuedState { get; private set; }


        public void Initialize(Dictionary<TStateEnum, TState> allStates, TStateEnum initialState)
        {
            _states = allStates;

            ChangeState(initialState);
        }

        public void ChangeState(TStateEnum state)
        {
            if (_states.TryGetValue(state, out TState newState))
            {
                QueuedState = state;
                if (CurrentState != newState)
                    CurrentState?.ExitState();

                QueuedState = default;
                CurrentState = newState;
                CurrentState.EnterState();
            }
            else
                Debug.LogError("Stato non trovato!");
        }

        public void ProcessFixedUpdate()
        {
            TStateEnum newState = CurrentState.ProcessFixedUpdate();
            if (newState != null)
                ChangeState(newState);
        }
    }
}