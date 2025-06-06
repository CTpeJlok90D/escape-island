using UnityEngine;

public class CharacterViewInstance : MonoBehaviour
{
    public CharacterInstance LinkedCharacter { get; private set; } 
    
    public CharacterViewInstance Instantiate(CharacterInstance linkedCharacter, Transform parent = null)
    {
        gameObject.SetActive(false);
        CharacterViewInstance result = Instantiate(this, parent);
        gameObject.SetActive(true);
        
        result.LinkedCharacter = linkedCharacter;
        result.gameObject.SetActive(true);

        return result;
    }
}