using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;

public class InventoryItemInstance : NetEntity<InventoryItemInstance>, IContainsInventoryItemData
{
    [SerializeField] private InventoryItemData _startData;
    
    private NetVariable<InventoryItemData> _data;
    public InventoryItemData Data => _data.Value;

    public override void Awake()
    {
        base.Awake();
        _data = new NetVariable<InventoryItemData>(_startData);
    }
}