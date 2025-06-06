using System.Collections;
using TNRD;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RandomView : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject[] _views;
    [SerializeField] private SerializableInterface<IContainsCharacter> _characterInstance;
    [SerializeField] private Transform _root;

    private IEnumerator Start()
    {
        while (_characterInstance.Value.Data == null)
        {
            yield return null;
        }
        
        System.Random random = new System.Random(_characterInstance.Value.Data.Value.ViewSeed);
        
        int viewIndex = random.Next(0, _views.Length);
        AssetReference viewAssetReference = _views[viewIndex];
        AsyncOperationHandle<GameObject> handle = viewAssetReference.LoadAssetAsync<GameObject>();
        handle.Completed += (h) =>
        {
            CharacterViewInstance view = h.Result.GetComponent<CharacterViewInstance>();
            view.Instantiate(_characterInstance.Value, _root);
        };
    }
}
