using UnityEngine;

public class WindowOrb : MonoBehaviour
{
    public Transform viewerCamera;
    public LayerMask windowMask;

    public float outsideOffset = 0.08f;
    public float maxRayDistance = 200f;

    Vector3 _startLocalPos;
    Transform _parent;

    void Awake()
    {
        _parent = transform.parent;
        _startLocalPos = transform.localPosition;

        if (viewerCamera == null && Camera.main != null)
            viewerCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (viewerCamera == null) return;

        Vector3 desired = _parent != null
            ? _parent.TransformPoint(_startLocalPos)
            : transform.position;

        Vector3 origin = viewerCamera.position;
        Vector3 toDesired = desired - origin;
        float dist = toDesired.magnitude;
        if (dist < 0.001f) return;

        Vector3 dir = toDesired / dist;

        if (Physics.Raycast(origin, dir, out RaycastHit hit,
                Mathf.Min(dist, maxRayDistance),
                windowMask, QueryTriggerInteraction.Collide))
        {
            Vector3 targetPos = hit.point + dir * outsideOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, 12f * Time.deltaTime);
        }
        else
        {
            transform.position = desired;
        }
    }
}