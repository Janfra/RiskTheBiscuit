using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : BaseShootingComponent, IAttackListener, ILookListener
{
    [SerializeField]
    private Transform _bulletOrigin;
    [SerializeField]
    private float _bulletOriginDistance;

    [SerializeField]
    private List<InterfaceReference<IShootable>> _shootablesPrefabs;
    private Vector2 _aimDirection;

    private const IInputListener.InputEvents _events = IInputListener.InputEvents.Started | IInputListener.InputEvents.Cancelled;
    public IInputListener.InputEvents TargetEventsFlag => _events;

    private bool _shootInputActive;
    private float _shootCooldown;

    private void Update()
    {
        _shootCooldown = Mathf.Max(0, _shootCooldown - Time.deltaTime);
    }

    protected override void OnShoot()
    {
        ShootData shootData;
        shootData.Direction = GetIntendedAimDirection();

        // May replace with pool
        foreach (var shootable in _shootablesPrefabs)
        {
            if (shootable.Object)
            {
                IShootable shootableInstance = (IShootable)Instantiate(shootable.Object, _bulletOrigin.position, _bulletOrigin.rotation);
                Debug.Assert(shootableInstance != null);
                shootableInstance.Shoot(shootData);
                _shootCooldown = _ShootRate;
            }
        }
    }

    public override void StartReload()
    {
        Debug.Log("Reload...");
        base.StartReload();
    }

    public override Vector2 GetIntendedAimDirection()
    {
        return _aimDirection;
    }

    public void OnLook(InputAction.CallbackContext context, Vector2 pointerWorldPosition)
    {
        _aimDirection = pointerWorldPosition - (Vector2)transform.position;
        SetBulletOrigin();
    }

    private void SetBulletOrigin()
    {
        Vector2 normalisedDirection = _aimDirection.normalized;
        Vector2 position = transform.position;
        _bulletOrigin.transform.position = position + (normalisedDirection * _bulletOriginDistance);

        float rotation = Mathf.Atan2(normalisedDirection.y, normalisedDirection.x) * Mathf.Rad2Deg;
        _bulletOrigin.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation - 90);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _bulletOriginDistance);
    }

    // Attack
    public void OnActionStarted(InputAction.CallbackContext context)
    {
        StartCoroutine(ShootAtRate());
    }

    public void OnActionPerformed(InputAction.CallbackContext context)
    {
        
    }

    public void OnActionCancelled(InputAction.CallbackContext context)
    {
        _shootInputActive = false;
    }
    // End attack

    protected IEnumerator ShootAtRate()
    {
        _shootInputActive = true;
        while (_shootInputActive)
        {
            if (_shootCooldown <= 0.0f)
            {
                TryShoot();
            }

            yield return null;
        }
    }
}
