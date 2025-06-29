using Core.Entities;
using Unity.Netcode;
using UnityEngine;

public class Interactable : NetEntity<Interactable>
{
    public delegate void InteractedDelegate(Interactable sender, GameObject interactionActivator);

    public event InteractedDelegate Interacted;
    public event InteractedDelegate InteractedLocal;

    public void Interact(GameObject interactionActivator)
    {
        InteractedLocal?.Invoke(this, interactionActivator);
        Interact_RPC(interactionActivator);
    }

    [Rpc(SendTo.Server)]
    private void Interact_RPC(NetworkObjectReference interactionActivatorReference)
    {
        if (interactionActivatorReference.TryGet(out NetworkObject interactionActivatorNetwork) == false)
        {
            return;
        }
        
        Interacted?.Invoke(this, interactionActivatorNetwork.gameObject);
    }
}