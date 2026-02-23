using UnityEngine;

public class MapSubmarineIcon : MonoBehaviour
{
    public FakeSubmarine submarine;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition = submarine.position;
        rect.localRotation = Quaternion.Euler(0, 0, -submarine.rotation);
    }
}