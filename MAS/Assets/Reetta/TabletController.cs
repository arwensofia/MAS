using UnityEngine;

public class TabletController : MonoBehaviour
{
    public float sendRate = 0.05f; // 20 times per second

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= sendRate)
        {
            timer = 0f;

            float rotate = HoldButton.CurrentRotationInput;

            if (rotate != 0f)
            {
                SendRotate(rotate);
            }
            else
            {
                SendRotate(0f);
            }
        }
    }

    void SendRotate(float value)
    {
        // Example payload
        string json = $"{{\"type\":\"rotate\",\"value\":{value}}}";

        // Send this over WebSocket / OSC / UDP
        Debug.Log(json);
    }
}