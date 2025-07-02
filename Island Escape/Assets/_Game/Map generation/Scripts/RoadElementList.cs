using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Road element list")]
public class RoadElementList : ScriptableObject
{
    [SerializeField] private RoadElement[] _roadElements;

    public IReadOnlyCollection<RoadElement> RoadElements => _roadElements;
}