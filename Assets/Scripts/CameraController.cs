using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private Transform _target;
    [SerializeField, Tooltip("Sets the camera reference distance. Must be negative to be behind.")]
    private float _cameraDistance;

    private void OnValidate()
    {
        if (_cam == null)
        {
            return;
        }

        _cam.transform.position = new Vector3(_cam.transform.position.x, _cam.transform.position.y, _cameraDistance);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = _target.position;
        _cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, _cameraDistance);
    }
}
