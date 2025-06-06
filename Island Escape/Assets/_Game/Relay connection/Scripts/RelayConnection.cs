using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Core.Connection
{
    public class RelayConnection
    {
        private static RelayConnection _instance;
        
        public static RelayConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RelayConnection();
                }
                return _instance;
            }
        }
        
        public event Action ConnectionStarted;
        public event Action Connected;
        public ReactiveField<string> JoinCode { get; private set; } = new();

        public async UniTask CreateRelay()
        {
            if (!AuthenticationService.Instance.IsAuthorized)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
            }
            
            try
            {
                ConnectionStarted?.Invoke();
                
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);

                JoinCode.Value = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                    );
                
                NetworkManager.Singleton.StartHost();
                
                Debug.Log($"Relay was created with join code: {JoinCode}. Already in clipboard");
                GUIUtility.systemCopyBuffer = JoinCode.Value;
                
                Connected?.Invoke();
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async UniTask JoinRelay(string joinCode)
        {
            if (!AuthenticationService.Instance.IsAuthorized)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
            }

            try
            {
                ConnectionStarted?.Invoke();
                
                JoinCode.Value = joinCode;
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                    );

                NetworkManager.Singleton.StartClient();
                
                Connected?.Invoke();
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
            }
        }
    }
}