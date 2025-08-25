using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    private bool _canMove = true;

    private void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        if (!_canMove) return;
        _rb.linearVelocity = direction.normalized * (speed * Time.fixedDeltaTime);
    }

    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        DisableMovement();
    }

    public void ClearVelocity()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    public void DisableMovement()
    {
        _canMove = false;
    }
}
