using Core.Players;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core
{
    public class NicknameField : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private string _fieldName = "nickname-text-field";

        private TextField _nicknameField;
    
        private void Awake()
        {
            _nicknameField = _uiDocument.rootVisualElement.Q<TextField>(_fieldName);
            _nicknameField.RegisterValueChangedCallback(OnValueChange);
            _nicknameField.value = "Player";
        }

        private void OnValueChange(ChangeEvent<string> eventCallback)
        {
            Player player = Player.Local;
            player.Nickname = eventCallback.newValue;
        }
    }
}