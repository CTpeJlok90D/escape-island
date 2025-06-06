using UnityEngine;
using UnityEngine.UIElements;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private GameBootstrap _gameBootstrap;
    [SerializeField] private string _buttonName = "start-game-button";

    private Button _button;
    
    private void Awake()
    {
        _button = _uiDocument.rootVisualElement.Q<Button>(_buttonName);
    }

    private void OnEnable()
    {
        _button.clicked += OnButtonClick;
    }

    private void OnDisable()
    {
        _button.clicked -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        _gameBootstrap.Launch();
    }
}
