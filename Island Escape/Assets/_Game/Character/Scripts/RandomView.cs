using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RandomView : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject[] _views;
    [SerializeField] private CharacterInstance _characterInstance;
    [SerializeField] private Transform _root;

    private void Start()
    {
        _ = RandomizeCharacter();
    }

    private async Task RandomizeCharacter()
    {
        System.Random random = new System.Random(_characterInstance.Data.ViewSeed);
        
        int viewIndex = random.Next(0, _views.Length);
        AssetReference viewAssetReference = _views[viewIndex];
        AsyncOperationHandle<GameObject> handle = viewAssetReference.LoadAssetAsync<GameObject>();

        await handle.ToUniTask();
        
        CharacterViewInstance view = handle.Result.GetComponent<CharacterViewInstance>();
        view.Instantiate(_characterInstance, _root);
    }
}
