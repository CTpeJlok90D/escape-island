using R3;
using Unity.Netcode.Custom;

public interface IContainsCharacter
{
    public NetVariable<CharacterData> Data { get; }
}