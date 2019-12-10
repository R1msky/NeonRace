using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target { get; set; }
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
      //  target = transform;
    }

    void FixedUpdate()
    {
         target = GameObject.FindGameObjectWithTag("Controllable Player").GetComponent<Transform>();
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    internal void SetTarget(Transform target)
    {
        this.target = target;
    }


}
