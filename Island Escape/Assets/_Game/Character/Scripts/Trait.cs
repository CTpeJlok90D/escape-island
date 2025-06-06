using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct Trait : INetworkSerializable
{
    public string ID;
    
    [FormerlySerializedAs("ConflictTraitsIDs")] [SerializeField]
    private string[] _conflictTraitsIDs;
    
    public float Wight;
    
    [SerializeField]
    [FormerlySerializedAs("PositiveSkillImpact")] 
    private Skill[] _positiveSkillImpact;
    
    [FormerlySerializedAs("NegativeSkillImpact")]
    [SerializeField]
    private Skill[] _negativeSkillImpact;
    
    public float MoveSpeedImpact;
    public float LoadCapacityImpact;

    public string[] ConflictTraitsIDs
    {
        get
        {
            _conflictTraitsIDs ??= Array.Empty<string>();
            return _conflictTraitsIDs;
        }
        set
        {
            _conflictTraitsIDs = value;
            _conflictTraitsIDs ??= Array.Empty<string>();
        }
    }
    
    public Skill[] PositiveSkillImpact
    {
        get
        {
            _positiveSkillImpact ??= Array.Empty<Skill>();
            return _positiveSkillImpact;
        }
        set
        {
            _positiveSkillImpact = value;
            _positiveSkillImpact ??= Array.Empty<Skill>();
        }
    }
    
    public Skill[] NegativeSkillImpact
    {
        get
        {
            _negativeSkillImpact ??= Array.Empty<Skill>();
            return _negativeSkillImpact;
        }
        set
        {
            _negativeSkillImpact = value;
            _negativeSkillImpact ??= Array.Empty<Skill>();
        }
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        _conflictTraitsIDs ??= Array.Empty<string>();
        _positiveSkillImpact ??= Array.Empty<Skill>();
        _negativeSkillImpact ??= Array.Empty<Skill>();
        
        FixedString64Bytes IdRef = new(ID);
        serializer.SerializeValue(ref IdRef);
        ID = IdRef.ToString();
        
        serializer.SerializeValue(ref Wight);
        serializer.SerializeValue(ref _positiveSkillImpact);
        serializer.SerializeValue(ref _negativeSkillImpact);
        serializer.SerializeValue(ref MoveSpeedImpact);
        serializer.SerializeValue(ref LoadCapacityImpact);
    }
}