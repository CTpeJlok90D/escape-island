using Core.Entities;
using R3;
using Unity.Netcode;
using Unity.Netcode.Custom;

public class CharacterInstance : NetEntity<CharacterInstance>, IContainsCharacter
{
    public ReactiveProperty<CharacterData> Data { get; private set; } = new();
    public NetVariable<bool> IsBot { get; private set; } = new();

    public CharacterInstance Instantiate(CharacterData data, ulong ownerShip, bool isBot = false)
    {
        if (NetworkManager.IsServer == false)
        {
            throw new NotServerException("Only server can spawn character instances.");
        }
        
        gameObject.SetActive(false);
        CharacterInstance characterInstance = Instantiate(this);
        gameObject.SetActive(true);
        
        characterInstance.Data.Value = data;
        characterInstance.IsBot = new(isBot);
        characterInstance.gameObject.SetActive(true);
        characterInstance.NetworkObject.SpawnWithOwnership(ownerShip, false);
        
        return characterInstance;
    }
}