using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    public bool CanMove => _canMove;
    public Rigidbody2D Rigidbody => _rb;

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
        _rb.linearVelocity = GetVelocity(direction, speed);
    }

    public void MoveTo(Vector2 target, float speed)
    {
        if (!_canMove) return;
        Internal_MoveTo(target, speed);
    }

    public bool TryMoveTo(Vector2 target, float speed)
    {
        if (!_canMove) return false;
        var resultData = Internal_MoveTo(target, speed);
        return (resultData.Velocity * Time.fixedDeltaTime).sqrMagnitude >= resultData.Distance.sqrMagnitude;
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

    private (Vector2 Distance, Vector2 Velocity) Internal_MoveTo(Vector2 target, float speed)
    {
        Vector2 distance = GetDistanceVector(target);
        Vector2 velocity = GetVelocity(distance, speed);
        _rb.linearVelocity = velocity;
        return (distance, velocity);
    }

    private Vector2 GetDistanceVector(Vector2 target)
    {
        Vector2 position = transform.position;
        return target - position;
    }

    private Vector2 GetVelocity(Vector2 direction, float speed)
    {
        return direction.normalized * (speed * Time.fixedDeltaTime);
    }

    protected Vector2 GetDirectionTo(Vector2 target)
    {
        Vector2 position = transform.position;
        return (target - position).normalized;
    }
}
