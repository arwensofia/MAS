using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UDPReciever))]
public class SubControl : MonoBehaviour
{
    [Header("Settings")] //speed, can tweak in inspector if needed
    public float _moveSpeed = 10f;
    public float _turnSpeed = 50f;

    [Header("Input")]
    [Tooltip("If true, keyboard can drive the sub when UDP input is zero (good for testing).")]
    public bool allowKeyboardFallback = true;

    [Tooltip("Deadzone for deciding whether UDP input is 'present'.")]
    public float udpDeadzone = 0.05f;

    
    private Rigidbody _rb;
    private UDPReciever _udpReciever;

    private Vector2 _moveInput;

    private void Awake() //faster to use than getcomponent from every frame
    {
        _rb = GetComponent<Rigidbody>();
        _udpReciever = GetComponent<UDPReciever>();
    }

    private void Update() //receive tablet input
    {
        Vector2 udp = _udpReciever != null ? _udpReciever.NetworkInput : Vector2.zero;
        //reads tablet input

        if (allowKeyboardFallback && udp.sqrMagnitude < (udpDeadzone * udpDeadzone))
        {
            udp = ReadKeyboardWASD();
        }

        _moveInput = Vector2.ClampMagnitude(udp, 1f);
        //WASD keyboard for testing
    }

    private Vector2 ReadKeyboardWASD() //this is for testing (keyboard)
    {
        if (Keyboard.current == null) return Vector2.zero;

        float x = 0f;
        float y = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed) y -= 1f;
        if (Keyboard.current.wKey.isPressed) y += 1f;

        Vector2 v = new Vector2(x, y);

        //normalize diagonals
        if (v.sqrMagnitude > 1f) v.Normalize();

        return v;
    }

    private void FixedUpdate() //submarine directions
    {
        //forward/back
        Vector3 force = transform.forward * _moveInput.y * _moveSpeed;
        _rb.AddForce(force, ForceMode.Acceleration);

        //turn left/right
        Vector3 turnForce = transform.up * _moveInput.x * _turnSpeed;
        _rb.AddTorque(turnForce, ForceMode.Acceleration);
    }
}

//tldr;
//script moves the submarine using tablet input and also uses keyboard (WASD keys) for testing
//every frame reads the input and adjusts accordingly
//physics (fixedupdate) handles forward/backward and turning