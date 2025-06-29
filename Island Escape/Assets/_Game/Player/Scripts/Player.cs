using System;
using Core.Entities;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core.Players
{
    public class Player : NetEntity<Player>, IContainsPlayer
    {
#if UNITY_EDITOR
        private const string NICKNAME_FORMAT = "Player: {0}";
#endif

        [SerializeField] private string _nickname = "Player";

        private NetworkVariable<FixedString64Bytes> _netNickname;

        public static Player Local { get; private set; }

        public NetBehaviourReference<CharacterInstance> LinkedCharacter { get; private set; }

        public static event Action<Player> LocalPlayerSpawned;

        public static event Action<Player> LocalPlayerDespawned;

        public static event Action<Player> Join;

        public static event Action<Player> Left;

        public bool IsLocal => this == Local;

        Player IContainsPlayer.Player => this;
        
        public event Action<string> NicknameChanged;
        
        public string Nickname
        {
            get => _nickname;
            set
            {
#if UNITY_EDITOR
                gameObject.name = string.Format(NICKNAME_FORMAT, value);
#endif
                _netNickname.Value = new FixedString64Bytes(value);
                _nickname = value;
            }
        }

        private void Start()
        {
            _nickname = _netNickname.Value.ToString();
            NicknameChanged?.Invoke(_nickname);
#if UNITY_EDITOR
            gameObject.name = string.Format(NICKNAME_FORMAT, _nickname);
#endif
        }

        public override void Awake()
        {
            base.Awake();
            _netNickname = new NetVariable<FixedString64Bytes>(writePerm:NetworkVariableWritePermission.Owner);
            _netNickname.OnValueChanged = OnNicknameChange;
            LinkedCharacter = new NetBehaviourReference<CharacterInstance>();
        }

        private void OnNicknameChange(FixedString64Bytes oldNickname, FixedString64Bytes newNickname) 
        {
            _nickname = newNickname.ToString();
            NicknameChanged?.Invoke(_nickname);
#if UNITY_EDITOR
            gameObject.name = string.Format(NICKNAME_FORMAT, _nickname);
#endif
        }

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
