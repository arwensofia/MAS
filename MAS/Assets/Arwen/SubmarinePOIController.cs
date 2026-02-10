using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class SubmarinePOIController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera defaultVCam;
    [SerializeField] private int activePriorityBoost = 20;
    //components to drag in inspector vcam default (forward POV)
    private class POIState
    {
        public Object owner;
        public CinemachineCamera poiCam;
        public int prevDefaultPriority;
        public int prevPoiPriority;
        //memory eg. POI_01, POI_02, etc.
    }

    private readonly List<POIState> _stack = new List<POIState>();
    //handles multiple points of views
    public void Activate(Object owner, CinemachineCamera poiCam)
        //to make the camera turn right
    {
        if (owner == null || poiCam == null || defaultVCam == null) return;
        //prevents null crash
        foreach (var s in _stack)
            if (s.owner == owner) return;

        var state = new POIState
        {
            owner = owner,
            poiCam = poiCam,
            prevDefaultPriority = defaultVCam.Priority,
            prevPoiPriority = poiCam.Priority
        };

        _stack.Add(state);
        //Unity would recognize which POI (point of interest) is active
        poiCam.Priority = defaultVCam.Priority + activePriorityBoost;
        //camera turns right
    }

    public void Deactivate(Object owner)
        //back 2 default
    {
        int index = _stack.FindIndex(s => s.owner == owner);
        if (index < 0) return;

        bool wasTop = index == _stack.Count - 1;
        var removed = _stack[index];
        _stack.RemoveAt(index);
        //true = POI currently driving the view
        //false = another POI
        //tldr; swapping/interchangeable POIs

        removed.poiCam.Priority = removed.prevPoiPriority;
        defaultVCam.Priority = removed.prevDefaultPriority;

        if (!wasTop) return;

        if (_stack.Count > 0)
        {
            var top = _stack[_stack.Count - 1];
            top.poiCam.Priority = defaultVCam.Priority + activePriorityBoost;
        }
    }
}
