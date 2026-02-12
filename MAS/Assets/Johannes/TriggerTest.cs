using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    public class AbsoluteTriggerTest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TRIGGER ENTER from " + other.name);
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("TRIGGER STAY from " + other.name);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("TRIGGER EXIT from " + other.name);
        }
    }
}
