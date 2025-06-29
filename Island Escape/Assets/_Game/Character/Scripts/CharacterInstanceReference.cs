using System;
using Core.Entities;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

public class CharacterInstanceReference : NetEntity<CharacterInstanceReference>, IContainsCharacter, IContainsCharacterInstance
{
    public NetBehaviourReference<CharacterInstance> InstanceReference { get; private set; } = new();
    public NetVariable<CharacterData> Data => InstanceReference.Reference.Data;
    public CharacterInstance CharacterInstance => InstanceReference.Reference;

    public CharacterInstanceReference Instantiate(CharacterInstance characterInstance, Vector3 position = default, Quaternion rotation = default)
    {
        if (NetworkManager.IsServer == false)
        {
            throw new NotServerException("Only server can spawn character instances.");
        }
        
        if (characterInstance.IsSpawned == false)
        {
            throw new ArgumentException("character instance is not spawned");
        }
        
        gameObject.SetActive(false);
        CharacterInstanceReference instanceReference = Instantiate(this, position, rotation);
        gameObject.SetActive(true);
        
        instanceReference.InstanceReference = new(characterInstance);
        instanceReference.gameObject.SetActive(true);
        instanceReference.NetworkObject.SpawnWithOwnership(characterInstance.NetworkObject.OwnerClientId);
        
        return instanceReference;
    }
}