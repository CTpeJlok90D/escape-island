using Core.Players;
using Unity.Netcode;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private LocationConfig _startLocationConfig;
    [SerializeField] private CharacterInstance _characterInstance_PREFAB;
    
    public void Launch()
    {
        if (NetworkManager.Singleton.IsServer == false)
        {
            throw new NotServerException("Only the server can launch game");
        }
        
        _startLocationConfig.Seed = Random.Range(int.MinValue, int.MaxValue);
        CreatePlayersCharacters();

        LocationConfigContainer.Instance.LocationConfig.Value = _startLocationConfig;
        WorldStateSwitcher.Instance.SwitchState(WorldState.Exploring);
    }

    private void CreatePlayersCharacters()
    {
        foreach (CharacterByTraitsFabric playerFabric in CharacterByTraitsFabric.Instances)
        {
            Player player = playerFabric.LinkedPlayer;
            CharacterData characterData = playerFabric.GeneratedCharacter.Value;
            player.LinkedCharacter.Reference = _characterInstance_PREFAB.Instantiate(characterData, playerFabric.OwnerClientId);
        }
    }
}