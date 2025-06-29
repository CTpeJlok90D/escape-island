using Core.Entities;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

public class InventoryItemInstance : NetEntity<InventoryItemInstance>, IContainsInventoryItemData, IContainsInventoryItemInstance
{
    [SerializeField] private InventoryItemData _startData;
    
    private NetVariable<InventoryItemData> _data;
    public InventoryItemData Data => _data.Value;
    public InventoryItemInstance InventoryItemInstanceReference => this;
    
    public bool IsInstance { get; private set; }

    public override void Awake()
    {
        base.Awake();
        _data = new NetVariable<InventoryItemData>(_startData);
        IsInstance = true;
    }
}