using UnityEngine;

public class CharacterViewInstance : MonoBehaviour
{
    public IContainsCharacter LinkedCharacter { get; private set; } 

    public CharacterViewInstance Instantiate(IContainsCharacter linkedCharacter, Transform parent = null)
    {
        gameObject.SetActive(false);
        CharacterViewInstance result = Instantiate(this, parent);
        gameObject.SetActive(true);
        
        result.LinkedCharacter = linkedCharacter;
        result.gameObject.SetActive(true);

        return result;
    }
}