using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Unity.Netcode.Custom
{
    [Serializable]
    public class NetVariable<T> : NetworkVariable<T>
    {
        private List<Action<T, T>> _listeners = new();
        
        public event Action<T, T> Changed
        {
            add => _listeners.Add(value);
            remove => _listeners.Remove(value);
        }

        public NetVariable() : base()
        {
            OnValueChanged = OnValueChange;
        }

        public NetVariable(
            T value = default,
            NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server) : base(value, readPerm, writePerm)
        {
            OnValueChanged = OnValueChange;
        }

        protected virtual void OnValueChange(T previousValue, T newValue)
        {
            foreach (Action<T, T> listener in _listeners.ToArray())
            {
                try
                {
                    listener?.Invoke(previousValue, newValue);   
                }
                catch (Exception e) 
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
