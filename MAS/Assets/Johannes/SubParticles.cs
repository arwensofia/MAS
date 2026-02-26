using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubmarineParticleController : MonoBehaviour
{
    public ParticleSystem forwardParticles;
    public float minForwardSpeed = 0.5f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Convert world velocity to local velocity
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);

        var emission = forwardParticles.emission;
        emission.enabled = localVelocity.z > minForwardSpeed;
    }
}