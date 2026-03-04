using UnityEngine;

public class Blink : MonoBehaviour
{
        public Color emissionColor = Color.red;
        public float blinkSpeed = 2f;      // How fast it blinks
        public float maxIntensity = 5f;    // How bright it gets

        private Material mat;

        void Start()
        {
            mat = GetComponent<Renderer>().material;
            mat.EnableKeyword("_EMISSION");
        }

        void Update()
        {
            float emission = Mathf.PingPong(Time.time * blinkSpeed, maxIntensity);
            Color finalColor = emissionColor * emission;
            mat.SetColor("_EmissionColor", finalColor);
        }
}
