using System;

namespace SyncedRush.Generics
{
    public abstract class BaseState<TStateEnum> where TStateEnum : Enum
    {
        public virtual void EnterState() { }

        public virtual void ExitState() { }

        // la keyword default indica il primo elemento dell'enum (in questo caso signifa "nessuno stato")
        public virtual TStateEnum ProcessUpdate() => default;

        public virtual TStateEnum ProcessFixedUpdate() => default;

    }
}