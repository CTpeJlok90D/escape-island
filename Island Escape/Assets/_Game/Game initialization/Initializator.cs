using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.GameInitialization
{
    public class Initializator : MonoBehaviour
    {
        [SerializeField] private string _mainMenuScene;
        private List<InitializationProcess> _processesToInit = new();

        public InitializationProcessList ProcessList { get; private set; }
        private void Awake()
        {
            _processesToInit = ProcessList.ToList();
        }

        private void OnEnable()
        {
            foreach (InitializationProcess process in ProcessList)
            {
                process.Completed += OnProcessComplete;
            }
        }

        private void Start()
        {
            if (_processesToInit.Count == 0)
            {
                OnInitializationComplete();
            }
        }

        private void OnDisable()
        {
            foreach (InitializationProcess process in ProcessList)
            {
                process.Completed -= OnProcessComplete;
            }
        }

        private void OnProcessComplete(InitializationProcess sender)
        {
            _processesToInit.Remove(sender);

            if (_processesToInit.Count == 0)
            {
                OnInitializationComplete();
            }
        }

        private void OnInitializationComplete()
        {
            SceneManager.LoadSceneAsync(_mainMenuScene, LoadSceneMode.Single);
        }
    }
}
