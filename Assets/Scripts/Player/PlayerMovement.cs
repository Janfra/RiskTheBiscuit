using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementComponent, IMoveListener
{
    [SerializeField]
    private float _moveSpeed = 10f;

    public void OnMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>(), _moveSpeed);
    }
}
