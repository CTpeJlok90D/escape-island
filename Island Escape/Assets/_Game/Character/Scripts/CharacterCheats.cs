using Core.Players;
using IngameDebugConsole;
using UnityEngine;

public class CharacterCheats : MonoBehaviour
{
    private void Start()
    {
        DebugLogConsole.AddCommand("Character.Regenerate", "Regenerate local character", ChangeNickname);
    }

    private void ChangeNickname()
    {
        CharacterByTraitsFabric nicknameContainer = Player.Local.GetComponent<CharacterByTraitsFabric>();
        nicknameContainer.RegenerateCharacter();

        Debug.Log($"Character for local player was regenerated");
    }
}
