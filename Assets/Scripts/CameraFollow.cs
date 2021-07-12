using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform lookAt;
    private Vector3 startOffset;

    // Start is called before the first frame update
    void Start()
    {
        lookAt = PlayerController.Instance.transform;
        startOffset = transform.position - lookAt.position;
    }

    private void LateUpdate()
    {
        transform.position = lookAt.position + startOffset;
    }
}
