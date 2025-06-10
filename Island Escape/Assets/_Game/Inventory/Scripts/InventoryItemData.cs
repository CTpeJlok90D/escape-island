using System;
using Unity.Netcode;

[Serializable]
public struct InventoryItemData : INetworkSerializable
{
    public string ID;
    public float Mass;
    public InventoryItemPawn ItemPawn;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref Mass);
    }
}