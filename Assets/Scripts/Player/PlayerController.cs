using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField]
    private List<InterfaceReference<ILookListener>> _lookListeners;
    [SerializeField]
    private List<InterfaceReference<IMoveListener>> _moveListeners;
    [SerializeField]
    private List<InterfaceReference<IAttackListener>> _attackListeners;

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        ValidateListeners(_lookListeners);
        ValidateListeners(_moveListeners);
        ValidateListeners(_attackListeners);
    }

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player.SetCallbacks(this);
        }

        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        if (_inputActions == null)
        {
            return;
        }

        _inputActions.Player.RemoveCallbacks(this);
        _inputActions.Player.Disable();
    }

    private void ValidateListeners<T>(List<InterfaceReference<T>> list) where T : class
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null || list[i].Value == null)
            {
                list.RemoveAt(i);
                Debug.LogWarning($"Removed null listener from list {list} at {i}");
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        foreach (var listener in _attackListeners)
        {
            if (listener == null || listener.Value == null)
            {
                Debug.LogError($"Listener of OnAttack is null.");
                continue;
            }

            listener.Value.OnAttack(context);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 pointerWorldPosition = ILookListener.PointerToWorldPosition(context, transform);
        foreach (var listener in _lookListeners)
        {
            if (listener == null || listener.Value == null)
            {
                Debug.LogError($"Listener of OnLook is null.");
                continue;
            }

            listener.Value.OnLook(context, pointerWorldPosition);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        foreach (var listener in _moveListeners)
        {
            if (listener == null || listener.Value == null)
            {
                Debug.LogError($"Listener of OnMove is null.");
                continue;
            }

            listener.Value.OnMove(context);
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }
}

public interface ILookListener
{
    void OnLook(InputAction.CallbackContext context, Vector2 pointerWorldPosition);

    public static Vector2 PointerToWorldPosition(InputAction.CallbackContext context, Transform transform)
    {
        Camera camera = Camera.main;
        Vector3 lookPosition = context.ReadValue<Vector2>();
        lookPosition.z = transform.position.z - camera.transform.position.z; // Distance between camera and 2D surface
        return camera.ScreenToWorldPoint(lookPosition);
    }
}

public interface IMoveListener
{
    void OnMove(InputAction.CallbackContext context);
}

public interface IAttackListener
{
    void OnAttack(InputAction.CallbackContext context);
}   