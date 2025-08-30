using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnerComponent : MonoBehaviour
{
    [Serializable]
    public class SpawnerConfiguration
    {
        [Serializable]
        public struct SpawnArea
        {
            public Vector2 Position;
            public Vector2 Size;
        }

        [Header("Spawn Configuration")]
        [SerializeField]
        private InterfaceReference<ISpawnable> _spawnPrefab;
        [SerializeField]
        private int _activeLimit;
        [SerializeField]
        private int _targetTotalSpawned;
        [SerializeField]
        private float _spawnDelay;
        public bool IsActive = true;
        [SerializeField]
        private LayerMask _spawnCheckMask;
        [SerializeField]
        private float _spawnCheckRadius;

        [SerializeField]
        private SpawnArea[] _spawnAreas;
        public SpawnArea[] Areas => _spawnAreas;

        public int ActiveObjects => _objectPool.CountActive;
        public float Delay => _spawnDelay;
        public int ActiveLimit => _activeLimit;
        public int TargetSpawned => _targetTotalSpawned;
        public bool HasValidPrefab => _spawnPrefab != null && _spawnPrefab.Object != null;

        private ObjectPool<ISpawnable> _objectPool;
        private bool _isSpawningOverTime;

        public void Initialise()
        {
            _objectPool = new ObjectPool<ISpawnable>(CreateSpawnable, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        }

        public bool TrySpawn(out ISpawnable spawnable)
        {
            if (!IsActive || !HasValidPrefab || ActiveObjects >= ActiveLimit) 
            {
                spawnable = null;
                return false;
            }

            spawnable = _objectPool.Get();
            spawnable.GameObject.transform.position = GetSpawnLocationWithoutCollision(GetRandomSpawnArea());
            return true;
        }

        public void StartSpawnOverTime(MonoBehaviour coroutineCaller, Action completedCallback)
        {
            if (_isSpawningOverTime)
            {
                return;
            }

            coroutineCaller.StartCoroutine(SpawnOverTime(completedCallback));
        }

        private IEnumerator SpawnOverTime(Action completedCallback)
        {
            _isSpawningOverTime = true;
            int spawnedCount = 0;

            while (spawnedCount < TargetSpawned)
            {
                yield return new WaitForSeconds(Delay);
                if (TrySpawn(out ISpawnable spawnable))
                {
                    spawnedCount++;
                }
            }

            _isSpawningOverTime = false;
            completedCallback?.Invoke();
        }

        private ISpawnable CreateSpawnable()
        {
            ISpawnable spawnable = (ISpawnable)Instantiate(_spawnPrefab.Object);
            spawnable.OriginPool = _objectPool;
            return spawnable;
        }

        private void OnReturnedToPool(ISpawnable spawnable)
        {
            spawnable.OnDespawn();
        }

        private void OnTakeFromPool(ISpawnable spawnable)
        {
            spawnable.OnSpawn();
        }

        private void OnDestroyPoolObject(ISpawnable spawnable)
        {
            spawnable.OnDestroyRequest();
        }

        private SpawnArea GetRandomSpawnArea()
        {
            return _spawnAreas[Random.Range(0, _spawnAreas.Length)];
        }

        private Vector2 GetSpawnLocationWithoutCollision(SpawnArea area)
        {
            Vector2 spawnPosition = area.Position;

            while (true)
            {
                float xPosition = Random.Range(0, area.Size.x) - area.Size.x * 0.5f;
                float yPosition = Random.Range(0, area.Size.y) - area.Size.y * 0.5f;
                spawnPosition = area.Position + new Vector2(xPosition, yPosition);
                bool isCollision = Physics2D.OverlapCircle(spawnPosition, _spawnCheckRadius, _spawnCheckMask.value);
                if (!isCollision)
                {
                    break;
                }
            }

            return spawnPosition;
        }
    }

    [SerializeField]
    private SpawnerConfiguration[] _spawnerConfigurations;
    private bool _isSpawning = false;
    public bool IsSpawning => _isSpawning;

    private void Awake()
    {
        foreach (var spawnConfig in _spawnerConfigurations)
        {
            spawnConfig.Initialise();
        }
    }

    private void Start()
    {
        TryStartSpawning();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public bool TryStartSpawning()
    {
        if (_isSpawning)
        {
            return false;
        }

        foreach (var spawnConfig in _spawnerConfigurations)
        {
            spawnConfig.StartSpawnOverTime(this, OnSpawningFinished);
        }
        return true;
    }

    public void OnSpawningFinished()
    {

    }

    private void OnDrawGizmos()
    {
        foreach (var configuration in _spawnerConfigurations) 
        {
            if (configuration == null || !configuration.HasValidPrefab) continue;

            foreach (var area in configuration.Areas)
            {
                Gizmos.DrawWireSphere(area.Position, 0.5f);
                Gizmos.DrawWireCube(area.Position, area.Size);
            }
        }
    }
}

public interface ISpawnable
{
    public IObjectPool<ISpawnable> OriginPool { get; set; }
    public GameObject GameObject { get; }

    public void OnDespawn();
    public void OnSpawn();
    public void OnDestroyRequest();
}
