using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float value; // -1 for left, +1 for right

    private bool isHeld;

    public static float CurrentRotationInput = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
        CurrentRotationInput = value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
        CurrentRotationInput = 0f;
    }
}