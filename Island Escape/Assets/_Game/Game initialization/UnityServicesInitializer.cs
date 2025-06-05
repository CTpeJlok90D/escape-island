using System;
using Unity.Services.Core;
using UnityEngine;

namespace Core.GameInitialization
{
    public class UnityServicesInitializer : MonoBehaviour
    {
        private void Start()
        {
            UnityServices.Initialized += OnInitialize;
            UnityServices.InitializeFailed += OnInitializeFailed;
            UnityServices.InitializeAsync();
        }

        private void OnInitializeFailed(Exception obj)
        {
            Debug.LogError($"Unity services initialization was failed. Error: {obj.Message}");
        }

        private void OnInitialize()
        {
            Debug.Log($"Unity services initialization is complete");
        }
    }
}
