using UnityEngine;

public class FakeSubmarine : MonoBehaviour
{
    public float moveSpeed = 50f;     // pixels per second (map space)
    public float rotateSpeed = 90f;   // degrees per second

    public Vector2 position;
    public float rotation; // degrees (0 = up)

    void Update()
    {
        // Read inputs from buttons
        float move = HoldMoveButton.CurrentMoveInput;
        float rotate = HoldButton.CurrentRotationInput;

        // Rotate
        rotation += rotate * rotateSpeed * Time.deltaTime;

        // Move forward in facing direction
        Vector2 forward = new Vector2(
            Mathf.Sin(rotation * Mathf.Deg2Rad),
            Mathf.Cos(rotation * Mathf.Deg2Rad)
        );

        position += forward * move * moveSpeed * Time.deltaTime;
    }
}