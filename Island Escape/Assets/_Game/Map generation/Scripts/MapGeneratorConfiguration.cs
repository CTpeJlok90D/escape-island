using System;
using Unity.Netcode;

[Serializable]
public struct MapGeneratorConfiguration : INetworkSerializable
{
    public int RoadLenghtSum;
    public int Seed;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref RoadLenghtSum);
        serializer.SerializeValue(ref Seed);
    }
}