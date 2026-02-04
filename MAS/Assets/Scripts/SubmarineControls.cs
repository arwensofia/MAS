using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class SubmarineControls : MonoBehaviour
{
    [Header("Settings")]

    public float _moveSpeed = 10f;
    public float _turnSpeed = 50f;

    private Rigidbody _rb;
    private SubmarineControllers _controls;
    private Vector2 _moveInput;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _controls = new SubmarineControllers();
    }

    private void OnEnable()
    {
        // Turn on controls
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        // Turn off controls
        _controls.Player.Disable();
    }

    void Update()
    {
        // Read the axis values (W and S = Y axis, A and D = X axis)
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // 1. Moving forward and back
        // Thrust forward and back in Y -axis input
        // transform.forward goes wherever object is facing

        Vector3 force = transform.forward * _moveInput.y * _moveSpeed;
        _rb.AddForce(force, ForceMode.Acceleration);

        // 2. Turning
        // Using A and D input for turning in X -axis

        float turn = _moveInput.x * _turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);

    }

}
