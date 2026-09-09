using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPool
{
    private Dictionary<AssetReference, Queue<GameObject>> _pool;

    public ObjectPool()
    {
        _pool = new Dictionary<AssetReference, Queue<GameObject>>();
    }

    public async UniTask<GameObject> GetAsync(AssetReference reference, CancellationToken cancellationToken)
    {
        if(!TryGetFromPool(reference, out var instance))
        {
            instance = await CreateAsync(reference, cancellationToken);
        }

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void Return(AssetReference reference, GameObject instance)
    {
        instance.gameObject.SetActive(false);
        _pool[reference].Enqueue(instance);
    }

    private bool TryGetFromPool(AssetReference reference, out GameObject instance)
    {
        instance = null;
        if (!_pool.ContainsKey(reference))
        {
            return false;
        }

        if(_pool[reference].Count <= 0)
        {
            return false;
        }

        instance = _pool[reference].Dequeue();
        return true;
    }

    private async UniTask<GameObject> CreateAsync(AssetReference reference, CancellationToken cancellationToken)
    {
        if(!_pool.ContainsKey(reference))
        {
            _pool.Add(reference, new Queue<GameObject>());
        }

        var gameObject = await Addressables.InstantiateAsync(reference);

        return gameObject;
    }
}
