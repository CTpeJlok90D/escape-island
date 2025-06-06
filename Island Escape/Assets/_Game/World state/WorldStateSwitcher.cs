using System;
using AYellowpaper.SerializedCollections;
using Core.Entities;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldStateSwitcher : NetEntity<WorldStateSwitcher>
{
    [SerializeField] private string _mainMenuSceneName = "Main menu";
    [SerializeField] private SerializedDictionary<WorldState, string> _scenesForStates;
    
    public NetVariable<WorldState> WorldState;

    public override void Awake()
    {
        NetworkObject.DestroyWithScene = false;
        WorldState = new NetVariable<WorldState>();
        base.Awake();
    }

    private void Start()
    {
        NetworkManager.OnClientStopped += OnClientStop;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (NetworkManager != null)
        {
            NetworkManager.OnClientStopped -= OnClientStop;
        }
    }

    private void OnClientStop(bool obj) => ReturnToMainMenu();
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void SwitchState(WorldState state)
    {
        if (IsServer == false)
        {
            throw new NotServerException("Only server is allowed to change world state.");
        }
        
        if (WorldState.Value == state)
        {
            throw new ArgumentException("State is already switched");
        }
        
        WorldState.Value = state;
        
        NetworkManager.SceneManager.LoadScene(_scenesForStates[state], LoadSceneMode.Single);
    }
}
