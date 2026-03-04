using UnityEngine;

public class InfoBoxTrigger : MonoBehaviour
{
    public GameObject TextBoxFin;
    public GameObject TextBoxEng;
    public GameObject Highlight;

    private void Awake()
    {
        TextBoxFin.SetActive(false);
        TextBoxEng.SetActive(false);
        Highlight.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
        TextBoxFin.SetActive(true);
        TextBoxEng.SetActive(true);
        Highlight.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        TextBoxFin.SetActive(false);
        TextBoxEng.SetActive(false);
        Highlight.SetActive(false);
    }

}
