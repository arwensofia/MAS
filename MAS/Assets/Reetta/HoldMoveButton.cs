using UnityEngine;
using UnityEngine.EventSystems;

public class HoldMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float value; // +1 forward, -1 backward

    public static float CurrentMoveInput = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        CurrentMoveInput = value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CurrentMoveInput = 0f;
    }
}
