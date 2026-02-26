using UnityEngine;

public class InfoBoxTrigger : MonoBehaviour
{
    public GameObject TextBoxFin;

    private void Awake()
    {
        TextBoxFin.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
        TextBoxFin.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        TextBoxFin.SetActive(false);
    }

}
