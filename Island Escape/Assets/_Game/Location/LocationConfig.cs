using System;
using Unity.Netcode;

[Serializable]
public struct LocationConfig : INetworkSerializable
{
    public LocationType LocationType;
    public int Seed;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref LocationType);
        serializer.SerializeValue(ref Seed);
    }
}