using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : BaseShootingComponent, IAttackListener
{
    protected override void OnShoot()
    {
        Debug.Log("Pew pew");
    }

    public override void StartReload()
    {
        Debug.Log("Reload...");
        base.StartReload();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryShoot();
        }
    }
}
