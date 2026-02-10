using Unity.Cinemachine;
using UnityEngine;

public class POIProximityByDistance : MonoBehaviour
{
    [SerializeField] private SubmarinePOIController controller;
    [SerializeField] private CinemachineCamera poiVCam;

    [SerializeField] private Transform submarineTransform;
    [SerializeField] private Transform poiCenter;

    [SerializeField] private float enterRadius = 6f;
    [SerializeField] private float exitRadius = 7f;

    private bool _active;

    private void OnValidate()
    {
        if (exitRadius < enterRadius)
            exitRadius = enterRadius;
    }

    private void Update()
    {
        if (!controller || !poiVCam || !submarineTransform || !poiCenter)
            return;

        float distance = Vector3.Distance(submarineTransform.position, poiCenter.position);

        if (!_active && distance <= enterRadius)
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
