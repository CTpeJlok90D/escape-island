using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public struct Skill : INetworkSerializable, IEquatable<Skill>
{
    [SerializeField] private SkillID _id;
    public SkillID ID => _id;
    public SkillGroup Group => ID.GetGroup();

    public Skill(SkillID id)
    {
        _id = id;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _id);
    }

    public bool Equals(Skill other)
    {
        return _id == other._id;
    }

    public override bool Equals(object obj)
    {
        return obj is Skill other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)_id;
    }
}