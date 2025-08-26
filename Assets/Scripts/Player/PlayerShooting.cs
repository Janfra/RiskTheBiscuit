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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryShoot();
        }
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
}
