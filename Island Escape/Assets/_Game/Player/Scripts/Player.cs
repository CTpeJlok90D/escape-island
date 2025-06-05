using System;
using Core.Entities;
using Unity.Netcode;

namespace Core.Players
{
    public class Player : NetEntity<Player>, IContainsPlayer
    {
        public static Player Local { get; private set; }

        public static event Action<Player> LocalPlayerSpawned;

        public static event Action<Player> LocalPlayerDespawned;

        public static event Action<Player> Join;

        public static event Action<Player> Left;

        public bool IsLocal => this == Local;

        Player IContainsPlayer.Player => this;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsLocalPlayer)
            {
                Local = this;
                LocalPlayerSpawned?.Invoke(this);
            }

            Join?.Invoke(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (this == Local) 
            {
                LocalPlayerDespawned?.Invoke(this);
            }
            
            Left?.Invoke(this);
        }
    }
}
