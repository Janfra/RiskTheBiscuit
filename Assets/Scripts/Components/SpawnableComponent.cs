using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class SpawnableComponent : MonoBehaviour, ISpawnable
{
    public IObjectPool<ISpawnable> OriginPool { get => _originPool; set => _originPool = value; }
    public GameObject GameObject { get => gameObject; }

    private IObjectPool<ISpawnable> _originPool;

    [SerializeField]
    private UnityEvent _unityOnSpawn;
    [SerializeField]
    private UnityEvent _unityOnDespawn;
    [SerializeField]
    private UnityEvent _unityOnDestroy;

    public void OnDespawn()
    {
        _unityOnDespawn?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnDestroyRequest()
    {
        _unityOnDestroy?.Invoke();
        Destroy(gameObject);
    }

    public void OnSpawn()
    {
        _unityOnSpawn?.Invoke();
        gameObject.SetActive(true);
    }

    public void RequestReturnToPool()
    {
        _originPool.Release(this);
    }
}
