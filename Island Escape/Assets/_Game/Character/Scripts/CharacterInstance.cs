using Core.Entities;

public class CharacterInstance : NetEntity<CharacterInstance>
{
    public CharacterData Data { get; private set;}
}