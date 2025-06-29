using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Players;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class CharacterByTraitsFabric : NetEntity<CharacterByTraitsFabric>
{
    [SerializeField] private int _traitMaxWight = 100;
    [SerializeField] private TraitSO[] _availableTraits;
    [SerializeField] private CharacterData _characterDataBase = new()
    {
        BaseLoadCapacity = 100,
        BaseMoveSpeed = 3,
    };
    
    [field: SerializeField] public NetVariable<CharacterData> GeneratedCharacter { get; private set; }

    public Player LinkedPlayer { get; private set; }
    
    public override void Awake()
    {
        base.Awake();

        LinkedPlayer = GetComponent<Player>();
        
        GeneratedCharacter = new NetVariable<CharacterData>(default, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
    }

    private void Start()
    {
        if (IsOwner)
        {
            RegenerateCharacter();
        }
    }

    public void RegenerateCharacter()
    {
        GeneratedCharacter.Value = GenerateCharacter();
    }

    public CharacterData GenerateCharacter()
    {
        Trait[] traits = PickRandomTraits();

        CharacterData data = _characterDataBase;
        
        data.TraitsIDs = traits.Select(x => x.ID).ToArray();
        data.BaseLoadCapacity += traits.Sum(x => x.LoadCapacityImpact);
        data.BaseMoveSpeed += traits.Sum(x => x.MoveSpeedImpact);
        data.ViewSeed = Guid.NewGuid().GetHashCode();
            
        List<Skill> skillsToAdd = new();
        
        foreach (Trait trait in traits)
        {
            foreach (Skill skill in trait.PositiveSkillImpact)
            {
                skillsToAdd.Add(skill);   
            }
        }
        
        foreach (Trait trait in traits)
        {
            foreach (Skill skill in trait.NegativeSkillImpact)
            {
                skillsToAdd.Remove(skill);
            }
        }
        
        data.Skills = skillsToAdd.ToArray();
        
        return data;
    }

    private Trait[] PickRandomTraits()
    {
        List<Trait> traits = new();
        List<TraitSO> availableTraits = new(_availableTraits);
        
        while (traits.Sum(x => x.Wight) < _traitMaxWight && availableTraits.Count > 0)
        {
            Trait trait = availableTraits.PopRandom().Trait;
            traits.Add(trait);

            availableTraits.RemoveAll(x => trait.ConflictTraitsIDs.Contains(x.Trait.ID));
            availableTraits.RemoveAll(x => x.Trait.ConflictTraitsIDs.Contains(trait.ID));
        }
        
        return traits.ToArray();
    }
}
