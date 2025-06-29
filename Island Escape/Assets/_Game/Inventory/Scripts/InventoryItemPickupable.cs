using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(IContainsInventoryItemInstance))]
public class InventoryItemPickupable : MonoBehaviour
{
    private Interactable _interactable;
    private IContainsInventoryItemInstance _inventoryItemInstance;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _inventoryItemInstance = GetComponent<IContainsInventoryItemInstance>();
    }

    private void OnEnable()
    {
        _interactable.Interacted += OnInteract;
    }

    private void OnDisable()
    {
        _interactable.Interacted -= OnInteract;
    }

    private void OnInteract(Interactable sender, GameObject interactionActivation)
    {
        if (interactionActivation.TryGetComponent(out IContainsCharacterInstance characterInstanceContainer) == false)
        {
            return;
        }
        
        CharacterInstance characterInstance = characterInstanceContainer.CharacterInstance;
        if (characterInstance.TryGetComponent(out CharacterInventory inventory) == false)
        {
            return;
        }
        
        inventory.AddItem(_inventoryItemInstance.InventoryItemInstanceReference);
    }
}
