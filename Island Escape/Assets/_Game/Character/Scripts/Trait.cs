using System;
using Unity.Collections;
using Unity.Netcode;

[Serializable]
public struct Trait : INetworkSerializable
{
    public string ID;
    public string[] ConflictTraitsIDs;
    public float Wight;
    public Skill[] PositiveSkillImpact;
    public Skill[] NegativeSkillImpact;
    public float MoveSpeedImpact;
    public float LoadCapacityImpact;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        FixedString64Bytes IdRef = new(ID);
        serializer.SerializeValue(ref IdRef);
        ID = IdRef.ToString();
        
        serializer.SerializeValue(ref Wight);
        serializer.SerializeValue(ref PositiveSkillImpact);
        serializer.SerializeValue(ref NegativeSkillImpact);
        serializer.SerializeValue(ref MoveSpeedImpact);
        serializer.SerializeValue(ref LoadCapacityImpact);
    }
}