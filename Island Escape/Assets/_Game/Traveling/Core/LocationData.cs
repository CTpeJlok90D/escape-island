using System;
using Unity.Netcode;

[Serializable]
public struct LocationData : INetworkSerializable
{
    public LocationType LocationType;
    public int MinDistanceToPlayerSpawn;
    public int MaxDistanceToPlayerSpawn;
    public int MinLocationTypeCount;
    public int MaxLocationTypeCount;
    public int MinDistanceBetweenSameBuildings;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref MinDistanceToPlayerSpawn);
        serializer.SerializeValue(ref MaxDistanceToPlayerSpawn);
        serializer.SerializeValue(ref LocationType);
        serializer.SerializeValue(ref MaxLocationTypeCount);
        serializer.SerializeValue(ref MinLocationTypeCount);
        serializer.SerializeValue(ref MinDistanceBetweenSameBuildings);
    }
}