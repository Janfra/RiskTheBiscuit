using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseShootingComponent : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    protected float _ReloadDuration;

    [SerializeField]
    protected int _MaxAmmo = 1;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _unityOnShoot;
    [SerializeField]
    private UnityEvent _unityOnReload;

    public bool HasAmmo => _CurrentAmmo > 0;
    public float ReloadDuration => _ReloadDuration;
    public int MaxAmmo => _MaxAmmo;
    public int CurrentAmmo => _CurrentAmmo;

    protected int _CurrentAmmo;

    protected virtual void Awake()
    {
        _CurrentAmmo = Mathf.Max(_MaxAmmo, 0);
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void TryShoot()
    {
        if (HasAmmo)
        {
            OnShoot();
            _unityOnShoot?.Invoke();
            ReduceAmmo();
        }
    }

    public virtual void StartReload()
    {
        _unityOnReload?.Invoke();
        if (_ReloadDuration > 0)
        {
            StartCoroutine(ReloadTimer());
        }
        else
        {
            OnReloadComplete();
        }
    }

    public abstract Vector2 GetIntendedAimDirection();
    protected abstract void OnShoot();
    protected virtual void ReduceAmmo()
    {
        _CurrentAmmo--;
        if (_CurrentAmmo <= 0)
        {
            StartReload();
        }
    }

    protected virtual void OnReloadComplete()
    {
        _CurrentAmmo = Mathf.Max(_MaxAmmo, 0);
    }

    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSecondsRealtime(_ReloadDuration);
        OnReloadComplete();
    }
}

public interface IShootable
{
    public void Shoot(ShootData data);
}

public struct ShootData
{
    public Vector2 Direction;
}