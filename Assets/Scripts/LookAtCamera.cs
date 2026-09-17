using UnityEngine;

public class LookAtCameraUI : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool reverseForward = true;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 direction = transform.position - targetCamera.transform.position;

        if (direction.sqrMagnitude <= 0.001f) return;

        if (reverseForward)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}