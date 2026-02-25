using Unity.Hierarchy;
using UnityEngine;

//Ensure rigidbody is attached to the object
[RequireComponent(typeof(Rigidbody))]

public class VerticalMovementController : MonoBehaviour
{
    [Header("Height Smoothing Settings")]
    public float _heightSmoothTime = 0.3f; // Time it takes to reach the new Y position

    private float _targetY;
    private float _yVelocity = 0.0f; // Used just for SmoothDamp (ease in and out)

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set the initial target Y
        _targetY = transform.position.y;

        // Just in case disable gravity on the player
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        // Calculate Y position
        float newY = Mathf.SmoothDamp(rb.position.y, _targetY, ref _yVelocity, _heightSmoothTime);

        // Apply the new Y position (newY) via physics engine
        // Grab the current X and Z (from movement script) and inject our new Y
        rb.MovePosition(new Vector3(rb.position.x, newY, rb.position.z));
    }

    // THIS IS OLD SCRIPT
    // LateUpdate ensures the Y height is applied after the other movement script
    //private void LateUpdate()
    //{
        // Smooth movement
       // float newY = Mathf.SmoothDamp(transform.position.y, _targetY, ref _yVelocity, _heightSmoothTime);

        // Apply the new Y position, but don't touch X or Z
       // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //}

    private void OnTriggerEnter(Collider other)
    {
        HeightZone zone = other.GetComponent<HeightZone>();
        if (zone != null)
        {
            _targetY = zone._targetHeight;
        }
    }
}
