using UnityEngine;

public class OrbFade : MonoBehaviour
{
    public Transform cameraTransform;
    public float fadeStartDistance = 3f;
    public float fadeEndDistance = 1f;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(cameraTransform.position, transform.position);

        float alpha = 1f;

        if (distance < fadeStartDistance)
        {
            alpha = Mathf.InverseLerp(fadeEndDistance, fadeStartDistance, distance);
        }

        Color newColor = originalColor;
        newColor.a = alpha;

        rend.material.color = newColor;
    }
}