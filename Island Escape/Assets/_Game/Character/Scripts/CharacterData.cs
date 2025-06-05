using System;
using Unity.Netcode;
using UnityEngine.Serialization;

[Serializable]
public struct CharacterData : INetworkSerializable
{
    public string[] TraitsIDs;
    public Skill[] Skills;
    public float BaseMoveSpeed;
    public float BaseLoadCapacity;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Skills);
        serializer.SerializeValue(ref BaseMoveSpeed);
        serializer.SerializeValue(ref BaseLoadCapacity);
    }
}