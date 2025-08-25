using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : BaseShootingComponent, IAttackListener, ILookListener
{
    private Vector2 _aimDirection;

    protected override void OnShoot()
    {
        Debug.Log("Pew pew");
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

    public void OnLook(InputAction.CallbackContext context)
    {
        Camera camera = Camera.main;
        Vector3 lookPosition = context.ReadValue<Vector2>();
        lookPosition.z = transform.position.z - camera.transform.position.z; // Distance between camera and 2D surface
        _aimDirection = camera.ScreenToWorldPoint(lookPosition);
    }
}
