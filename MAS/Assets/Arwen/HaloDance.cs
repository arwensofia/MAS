using UnityEngine;

public class Hale : MonoBehaviour
{
    [Header("Base Size")]
    public float baseSize = 1.2f;

    [Header("Pulse")]
    public float speed = 3f;
    public float scalePulse = 6f;   // was 0.08

    Vector3 _baseScale;

    void Awake()
    {
        _baseScale = Vector3.one * baseSize;
        transform.localScale = _baseScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 5f) * 0.15f;
        transform.localScale = _baseScale * (5f + t * scalePulse);
    }
}