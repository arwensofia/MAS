using UnityEngine;

public class SubmarineSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");   // W / S
        float turnInput = Input.GetAxis("Horizontal"); // A / D

        bool isMoving = Mathf.Abs(moveInput) > 0.01f || Mathf.Abs(turnInput) > 0.01f;

        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}