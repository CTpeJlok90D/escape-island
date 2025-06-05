using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Core.Players
{
    public class NicknameContainer : NetworkBehaviour
    {
#if UNITY_EDITOR
        private const string NICKNAME_FORMAT = "Player: {0}";
#endif

        [SerializeField] private string _value;
        
        private NetworkVariable<FixedString64Bytes> _netNickname = new(writePerm:NetworkVariableWritePermission.Owner);

        public event Action<string> Changed;
        
        public string Value
        {
            get => _value;
            set
            {
#if UNITY_EDITOR
                gameObject.name = string.Format(NICKNAME_FORMAT, value);
#endif
                _netNickname.Value = new FixedString64Bytes(value);
                _value = value;
            }
        }

        private void Start()
        {
            _value = _netNickname.Value.ToString();
            Changed?.Invoke(_value);
#if UNITY_EDITOR
            gameObject.name = string.Format(NICKNAME_FORMAT, _value);
#endif
        }

        private void Awake()
        {
            _netNickname.OnValueChanged = OnNicknameChange;
        }

        private void OnNicknameChange(FixedString64Bytes oldNickname, FixedString64Bytes newNickname) 
        {
            _value = newNickname.ToString();
            Changed?.Invoke(_value);
#if UNITY_EDITOR
            gameObject.name = string.Format(NICKNAME_FORMAT, _value);
#endif
        }
    }
}
