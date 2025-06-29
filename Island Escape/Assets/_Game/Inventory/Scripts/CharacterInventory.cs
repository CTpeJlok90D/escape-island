using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEditor;
using UnityEngine;

public class CharacterInventory : NetEntity<CharacterInventory>
{
    [SerializeField] private CharacterInstance _character;
    private NetworkList<NetworkObjectReference> _netItems;
    public IEnumerable<InventoryItemInstance> Items => _netItems.ToEnumerable<InventoryItemInstance>();
    public float Mass => Items.Sum(x => x.Data.Mass);
    public float Capacity => _character.Data.Value.BaseLoadCapacity;

    public override void Awake()
    {
        base.Awake();
        _netItems = new(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }

    public void AddItem(InventoryItemPawn itemPawn)
    {
        InventoryItemInstance itemInstance = itemPawn.InventoryItemInstanceReference;
        
        DestroyPickedUpItemRPC(itemPawn);
        
        _netItems.Add(itemInstance.NetworkObject);
        ChangeOwnerShipRPC(itemInstance);
    }
    
    public void AddItem(InventoryItemInstance item)
    {
        if (item.IsInstance)
        {
            AddInstance(item);
        }
        else
        {
            InstantiateAndAddItem(item);
        }
    }

    private void AddInstance(InventoryItemInstance item)
    {
        _netItems.Add(item.NetworkObject);
        
        InventoryItemPawn inventoryItemPawn = InventoryItemPawn.Instances.FirstOrDefault(x => x.InventoryItemInstanceReference == item);
        if (inventoryItemPawn == null)
        {
            Debug.LogWarning($"Pawn for item instance {item} was not found");
        }
        else
        {
            DestroyPickedUpItemRPC(inventoryItemPawn);
        }
        
        ChangeOwnerShipRPC(item);
    }

    [Rpc(SendTo.Server)]
    private void ChangeOwnerShipRPC(NetworkBehaviourReference obj)
    {
        NetworkBehaviour item; obj.TryGet(out item);

        if (item == null || item is not InventoryItemInstance)
        {
            throw new ArgumentException("Item not found");
        }
        
        item.NetworkObject.ChangeOwnership(NetworkObject.OwnerClientId);
    }
    
    private void InstantiateAndAddItem(InventoryItemInstance item)
    {
        if (IsServer == false)
        {
            throw new NotServerException("Only server can instantiate items");
        }
        
        InventoryItemInstance instance = Instantiate(item);
        instance.NetworkObject.SpawnWithOwnership(NetworkObject.OwnerClientId);
    }

    [Rpc(SendTo.Server)]
    private void DestroyPickedUpItemRPC(NetworkBehaviourReference item)
    {
        item.TryGet(out NetworkBehaviour itemPawn);
        
        if (itemPawn == null || itemPawn is not InventoryItemPawn)
        {
            throw new ArgumentException("Item not found");
        }
        
        itemPawn.NetworkObject.Despawn();
    }

    public void DropItem(InventoryItemInstance item)
    {
        InventoryItemInstance[] items = _netItems.ToEnumerable<InventoryItemInstance>().ToArray();
        
        if (items.Contains(item) == false)
        {
            throw new ArgumentException("Inventory dont contains this item");
        }
    }

    public override string ToString()
    {
        return string.Join(",", Items.Select(x => x.Data.ID));
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterInventory))]
    private class CEditor : Editor
    {
        private new CharacterInventory target => base.target as CharacterInventory; 
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = false;
            foreach (InventoryItemInstance item in target.Items)
            {
                EditorGUILayout.ObjectField("", item, typeof(InventoryItemInstance), true);
            }
            GUI.enabled = true;
        }
    }
#endif
}