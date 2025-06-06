using System;
using System.Linq;
using Core.Players;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneratedCharacterWindow : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private string _viewSeedLabelName = "view-seed-label";
    [SerializeField] private string _traitsLabelName = "traits";
    [SerializeField] private string _skillsLabelName = "skills";
    [SerializeField] private string _moveSpeedLabelName = "move-speed-label";
    [SerializeField] private string _loadCapacity = "load-capacity-label";
    [SerializeField] private string _regenerateButtonName = "regenerate-character-button";
    [Space]
    [SerializeField] private string _viewSeedLabelFormat = "View seed: {0}";
    [SerializeField] private string _traitsLabelFormat = "Trait's: {0}";
    [SerializeField] private string _skillsLabelFormat = "Skills: {0}";
    [SerializeField] private string _moveSpeedLabelFormat = "MoveSpeed: {0}";
    [SerializeField] private string _loadCapacityLabelFormat = "LoadCapacity: {0}";

    private Label _viewSeedLabel;
    private Label _traitsLabel;
    private Label _skillsLabel;
    private Label _moveSpeedLabel;
    private Label _loadCapacityLabel;
    private Button _regenerateButton;

    private void Awake()
    {
        _viewSeedLabel = _uiDocument.rootVisualElement.Q<Label>(_viewSeedLabelName);
        _traitsLabel = _uiDocument.rootVisualElement.Q<Label>(_traitsLabelName);
        _skillsLabel = _uiDocument.rootVisualElement.Q<Label>(_skillsLabelName);
        _moveSpeedLabel = _uiDocument.rootVisualElement.Q<Label>(_moveSpeedLabelName);
        _loadCapacityLabel = _uiDocument.rootVisualElement.Q<Label>(_loadCapacity);
        _regenerateButton = _uiDocument.rootVisualElement.Q<Button>(_regenerateButtonName);
    }

    private void OnEnable()
    {
        Player.LocalPlayerSpawned += OnLocalPlayerSpawn;
        _regenerateButton.clicked += OnRegenerateButtonClick;
    }

    private void OnDisable()
    {
        Player.LocalPlayerSpawned -= OnLocalPlayerSpawn;
        _regenerateButton.clicked -= OnRegenerateButtonClick;
    }

    private void OnRegenerateButtonClick()
    {
        if (Player.Local == null)
        {
            throw new NullReferenceException("Local player now found");
        }
        
        CharacterByTraitsFabric fabric = Player.Local.GetComponent<CharacterByTraitsFabric>();
        fabric.RegenerateCharacter();
        UpdateCharacterInfo();
    }

    private void OnLocalPlayerSpawn(Player obj)
    {
        _ = UpdateWithDelay();
    }

    private async UniTask UpdateWithDelay()
    {
        await UniTask.DelayFrame(1);
        UpdateCharacterInfo();
    }

    private void UpdateCharacterInfo()
    {
        if (Player.Local == null)
        {
            throw new NullReferenceException("Local player now found");
        }
        
        CharacterByTraitsFabric fabric = Player.Local.GetComponent<CharacterByTraitsFabric>();
        CharacterData characterData = fabric.GeneratedCharacter.Value;

        _viewSeedLabel.text = string.Format(_viewSeedLabelFormat, characterData.ViewSeed.ToString());
        _moveSpeedLabel.text = string.Format(_moveSpeedLabelFormat, characterData.BaseMoveSpeed.ToString());
        _loadCapacityLabel.text = string.Format(_loadCapacityLabelFormat, characterData.BaseLoadCapacity.ToString());
        _traitsLabel.text = string.Format(_traitsLabelFormat, string.Join(", ", characterData.TraitsIDs));
        _skillsLabel.text = string.Format(_skillsLabelFormat, string.Join(", ", characterData.SortedSkills.Select(x => $"{x.Key}: {x.Value}")));
    }
}
