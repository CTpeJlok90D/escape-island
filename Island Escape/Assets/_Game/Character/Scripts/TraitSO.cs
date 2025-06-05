using System;
using UnityEngine;

[Icon("Assets/_Game/Character/Editor/icons8-brain-48.png")]
[CreateAssetMenu(fileName = "Trait", menuName = "Game/Character/Trait")]
public class TraitSO : ScriptableObject
{
    [field: SerializeField] public Trait Trait { get; private set; }

    private void OnValidate()
    {
        Trait trait = Trait;
        trait.ID = name;
        Trait = trait;
    }
}