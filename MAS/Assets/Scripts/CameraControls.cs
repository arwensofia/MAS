using UnityEngine;
using Unity.Cinemachine;


public class CameraControls : MonoBehaviour
{
    public CinemachineCamera[] _kamerat;


    //This changes the Priority value of the camera position that the user wants to be active
    //The Priority value changes to 0 in others in the list exept for the active one
    public void VaihdaKameraa(int index)
    {
        for (int i = 0; i < _kamerat.Length; i++)
        {
            _kamerat[i].Priority = (i == index) ? 20 : 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Alpha1)) VaihdaKameraa(0);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) VaihdaKameraa(1);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) VaihdaKameraa(2);
    }

    //These below are to be assigned to the different button event actions in Unity
    public void EtuKamera()
    {
        VaihdaKameraa(0);
    }

    public void KeskiKamera()
    {
        VaihdaKameraa(1);
    }

    public void TakaKamera()
    {
        VaihdaKameraa(2);
    }
}
