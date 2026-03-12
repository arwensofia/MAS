using UnityEngine;

public class HaloLiftOnApproach : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform submarine;
    [SerializeField] private Transform poiCenter;

    [Header("Distance")]
    [SerializeField] private float enterRadius = 6f;
    [SerializeField] private float exitRadius = 7f;

    [Header("Lift")]
    [SerializeField] private float liftAmount = 3f;
    [SerializeField] private float moveSmoothTime = 0.25f;

    [Header("Bounce")]
    [SerializeField] private float bounceHeight = 0.35f;
    [SerializeField] private float bounceSpeed = 4f;

    private Vector3 startLocalPos;
    private Vector3 targetLocalPos;
    private Vector3 velocity;

    private bool lifted;
    private float bounceTimer;

    private void Start()
    {
        startLocalPos = transform.localPosition;
        targetLocalPos = startLocalPos;

        if (submarine == null)
        {
            GameObject sub = GameObject.Find("Submarine_FIX");
            if (sub != null)
                submarine = sub.transform;
        }
    }

    private void Update()
    {
        if (submarine == null || poiCenter == null)
            return;

        float distance = Vector3.Distance(submarine.position, poiCenter.position);

        if (!lifted && distance <= enterRadius)
        {
            lifted = true;
            bounceTimer = 0f;
        }
        else if (lifted && distance >= exitRadius)
        {
            lifted = false;
            bounceTimer = 0f;
        }

        Vector3 baseTarget = lifted
            ? startLocalPos + Vector3.up * liftAmount
            : startLocalPos;

        // little bounce when lifted
        if (lifted)
        {
            bounceTimer += Time.deltaTime * bounceSpeed;
            float bounce = Mathf.Sin(bounceTimer) * bounceHeight;
            targetLocalPos = baseTarget + Vector3.up * bounce;
        }
        else
        {
            targetLocalPos = baseTarget;
        }

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetLocalPos,
            ref velocity,
            moveSmoothTime
        );
    }
}