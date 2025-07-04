using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core.Entities
{
    [DefaultExecutionOrder(-1)]
    public abstract class NetEntity<T> : NetworkBehaviour where T : NetEntity<T>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void DomainReset()
        {
            _instances = new();
        }

        private static List<T> _instances = new();

        public static IReadOnlyList<T> Instances => _instances;

        public static T Instance
        {
            get
            {
                if (_instances.Count > 1)
                {
                    throw new InvalidOperationException($"{typeof(T).Name} exists more than once");
                }
                
                if (_instances.Count == 0)
                {
                    throw new InvalidOperationException($"Instances of {typeof(T).Name} do not exists");
                }
                
                return _instances[0];
            }
        }

        public static bool Exists => _instances.Any(); 

        public Guid EntityID { get; private set; }
        
        public virtual void Awake()
        {
            EntityID = Guid.NewGuid();
        }
        
        protected virtual void OnEnable()
        {
            _instances.Add((T)this);
        }

        protected virtual void OnDisable()
        {
            _instances.Remove((T)this);
        }
    }
}
