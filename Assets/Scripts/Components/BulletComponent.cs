using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletComponent : MonoBehaviour, IShootable
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _speed;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _unityOnShot;

    private void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    public void Shoot(ShootData ShootData)
    {
        _rb.AddForce(ShootData.Direction.normalized * _speed);
        _unityOnShot?.Invoke();
    }

    public void OnAffectedEntity()
    {
        Destroy(gameObject);
    }
}
