using Unity.Cinemachine;
using UnityEngine;

public class POIProximityByDistance : MonoBehaviour
    //for inspector; click and drag
{
    [SerializeField] private SubmarinePOIController controller;
    [SerializeField] private CinemachineCamera poiVCam;

    [SerializeField] private Transform submarineTransform;
    [SerializeField] private Transform poiCenter;

    [SerializeField] private float enterRadius = 6f;//distance threshold
    [SerializeField] private float exitRadius = 7f;//also distance threshold

    private bool _active;

    private void OnValidate() //when submarine enters or exits the area, keeps the values valid
    {
        if (exitRadius < enterRadius)
            exitRadius = enterRadius;
    }

    private void Update() //runs once per frame, checks distance every time
    {
        if (!controller || !poiVCam || !submarineTransform || !poiCenter)
            return;
        //safety check

        float distance = Vector3.Distance(submarineTransform.position, poiCenter.position);
        //distance calculation, proximity

        if (!_active && distance <= enterRadius) //condition, if active/if not
        {
            _active = true;
            controller.Activate(this, poiVCam);
        }
        else if (_active && distance >= exitRadius)
        {
            _active = false;
            controller.Deactivate(this);
        }
    }
}

//tldr;
//distance-based trigger that tells your controller
//to turn this specific POI camera when the submarine comes close
//or to turn it off when it moves away