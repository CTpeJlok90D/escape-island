using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

public class CharacterInventory : NetEntity<CharacterInventory>
{
    [SerializeField] private CharacterInstance _character;
    private NetworkList<NetworkObjectReference> _netItems;
    public IEnumerable<InventoryItemInstance> Items => _netItems.ToEnumerable<InventoryItemInstance>();
    public float Mass => Items.Sum(x => x.Data.Mass);
    public float Capacity => _character.Data.Value.BaseLoadCapacity;
}