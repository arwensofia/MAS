using UnityEngine;

public class InfoBoxTrigger : MonoBehaviour
{
    public GameObject TextBox;

    private void Awake()
    {
        Debug.Log("Script works wooo");
        TextBox.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
            Debug.Log("Something entered trigger: " + other.name);
            TextBox.SetActive(true);
    }


}
