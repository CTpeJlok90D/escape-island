using Core.Connection;
using Cysharp.Threading.Tasks;
using IngameDebugConsole;
using UnityEngine;

public class RelayConnectionCheats : MonoBehaviour
{
    private void Start()
    {
        DebugLogConsole.AddCommand<string>("Relay.Connect", "Connect to the host by join code", Connect);
        DebugLogConsole.AddCommand("Relay.Host", "Create local server", Host);
        DebugLogConsole.AddCommand("Relay.GetCode", "Get join code to the connected server", GetCode);
    }

    private void Host()
    {
        _ = HostAsync();
    }

    private async UniTask HostAsync()
    {
        await RelayConnection.Instance.CreateRelay();
        Debug.Log("Relay connection created");
        GetCode();
    }

    private void Connect(string joinCode)
    {
        _ = ConnectAsync(joinCode);
    }

    private async UniTask ConnectAsync(string joinCode)
    {
        await RelayConnection.Instance.JoinRelay(joinCode);
        Debug.Log("Connected to the host by join code");
    }
    
    private void GetCode()
    {
        Debug.Log($"Join code: {RelayConnection.Instance.JoinCode}");
    }
}
