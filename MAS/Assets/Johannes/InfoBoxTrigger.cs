using UnityEngine;

public class InfoBoxTrigger : MonoBehaviour
{
    public GameObject TextBoxFin;
    public GameObject TextBoxEng;

    private void Awake()
    {
        TextBoxFin.SetActive(false);
        TextBoxEng.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
        TextBoxFin.SetActive(true);
        TextBoxEng.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        TextBoxFin.SetActive(false);
        TextBoxEng.SetActive(false);
    }

}
