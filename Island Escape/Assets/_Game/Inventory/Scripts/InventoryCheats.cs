using Core.Players;
using IngameDebugConsole;
using UnityEngine;

public class InventoryCheats : MonoBehaviour
{
    private void Start()
    {
        DebugLogConsole.AddCommand<PlayerSelector>("Inventory.Show", "Show inventory of the player", ShowInventory);
    }

    private void ShowInventory(PlayerSelector playerSelector)
    {
        foreach (Player player in playerSelector)
        {
            CharacterInventory inventory = player.LinkedCharacter.Reference.GetComponent<CharacterInventory>();
            string inventoryString = inventory.ToString();
            Debug.Log($"{player.Nickname} inventory: {inventoryString}");
        }
    }
}