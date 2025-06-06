using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct CharacterData : INetworkSerializable
{
    public string[] TraitsIDs;
    public Skill[] Skills;
    public float BaseMoveSpeed;
    public float BaseLoadCapacity;
    public int ViewSeed;
    
    public Dictionary<SkillID, int> SortedSkills
    {
        get
        {
            Dictionary<SkillID, int> sorted = new Dictionary<SkillID, int>();
            foreach (Skill skill in Skills)
            {
                if (sorted.ContainsKey(skill.ID))
                {
                    continue;
                }
                int count = Skills.Count(x => x.ID == skill.ID);
                sorted.Add(skill.ID, count);
            }
            
            return sorted;
        }
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        TraitsIDs ??= Array.Empty<string>();
        Skills ??= Array.Empty<Skill>();
        
        FixedString64Bytes[] traitsIDs = {};
        
        if (serializer.IsWriter)
        {
            traitsIDs = TraitsIDs.Select(x => new FixedString64Bytes(x)).ToArray();
        }
        
        int length = TraitsIDs.Length;
        serializer.SerializeValue(ref length);

        if (serializer.IsReader)
        {
            traitsIDs = new FixedString64Bytes[length];
        }
        
        for (int i = 0; i < length; i++)
        {
            serializer.SerializeValue(ref traitsIDs[i]);
        }

        if (serializer.IsReader)
        {
            TraitsIDs = traitsIDs.Select(x => x.ToString()).ToArray();
        }
        
        serializer.SerializeValue(ref Skills);
        serializer.SerializeValue(ref BaseMoveSpeed);
        serializer.SerializeValue(ref BaseLoadCapacity);
        serializer.SerializeValue(ref ViewSeed);
    }
}