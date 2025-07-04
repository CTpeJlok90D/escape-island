using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public record LocationsGeneratorConfig : INetworkSerializable
{
    public int Seed;
    public Vector2Int MapSize;
    public LocationData[] PossibleLocations;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PossibleLocations);
        serializer.SerializeValue(ref Seed);
        serializer.SerializeValue(ref MapSize);
    }
}