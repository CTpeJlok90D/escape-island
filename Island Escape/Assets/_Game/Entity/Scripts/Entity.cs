using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Entities
{
    [DefaultExecutionOrder(-1)]
    public class Entity<T> : MonoBehaviour where T : Entity<T>
    {
        private static List<T> _instances = new();
        public event Action<T> Spawned;
        public event Action<T> Despawned;
        
        public static IReadOnlyList<T> Instances
        {
            get { return _instances.ToArray(); }
        }

        protected virtual void OnEnable()
        {
            _instances.Add((T)this);
            Spawned?.Invoke((T)this);
        }

        protected virtual void OnDisable()
        {
            _instances.Remove((T)this);
            Despawned?.Invoke((T)this);
        }
    }
}
