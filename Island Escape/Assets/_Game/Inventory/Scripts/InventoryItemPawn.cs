using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;

[Icon("Assets/_Game/Inventory/Editor/icons8-flashlight-48.png")]
public class InventoryItemPawn : NetEntity<InventoryItemPawn>, IContainsInventoryItemData, IContainsInventoryItemInstance
{
    [field: SerializeField] private InventoryItemInstance _instanceToSpawn;
    public NetBehaviourReference<InventoryItemInstance> Reference { get; private set; }

    public InventoryItemData Data => InventoryItemInstanceReference.Data;
    public InventoryItemInstance InventoryItemInstanceReference => Reference.Reference;

    private bool _isNewInstance = true;
    
    public InventoryItemPawn Instantiate(InventoryItemInstance reference)
    {
        gameObject.SetActive(false);
        InventoryItemPawn pawn = Instantiate(this);
        gameObject.SetActive(true);
        
        pawn.Reference = new(reference);
        pawn._isNewInstance = false;
        pawn.gameObject.SetActive(true);
        return pawn;
    }

    public override void Awake()
    {
        base.Awake();
        Reference ??= new();
    }

    public void Start()
    {
        if (IsServer == false)
        {
            return;
        }

        if (_isNewInstance)
        {
            InventoryItemInstance newInstance = Object.Instantiate(_instanceToSpawn);
            newInstance.NetworkObject.Spawn();
            Reference.Reference = newInstance;
        }
    }
}