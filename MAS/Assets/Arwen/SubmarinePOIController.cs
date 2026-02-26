using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class SubmarinePOIController : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera defaultVCam;
    [SerializeField] private int activePriorityBoost = 20;

    [Header("Teleport (Jump to POIs)")]
    [SerializeField] private Transform submarineRoot;
    [SerializeField] private Rigidbody submarineRb;
    [SerializeField] private CinemachineBrain brain;

    [Tooltip("Put your POI_Center transforms here in the order you want (0..11).")]
    [SerializeField] private Transform[] poiCenters;

    [Tooltip("If true: X at last goes to first, Z at first goes to last.")]
    [SerializeField] private bool wrapAround = true;

    [Tooltip("Start index used when pressing X/Z the first time.")]
    [SerializeField] private int startIndex = 0;
    //for inspector; components to drag

    private int _currentIndex;
    private bool _initializedIndex;

    private class POIState
    {
        public Object owner;
        public CinemachineCamera poiCam;
        public int prevDefaultPriority;
        public int prevPoiPriority;
        //memory eg. POI_01, POI_02, etc.
    }

    private readonly List<POIState> _stack = new List<POIState>();
    //handles multiple POVs

    private void Awake()
    {
        _currentIndex = Mathf.Clamp(startIndex, 0, Mathf.Max(0, (poiCenters?.Length ?? 1) - 1));
    }

    private void Update() //keyboard buttons for testing
    {
        // X = ascending (next)
        if (Input.GetKeyDown(KeyCode.X)) JumpNext();

        // Z = descending (previous)
        if (Input.GetKeyDown(KeyCode.Z)) JumpPrevious();
    }

    public void Activate(Object owner, CinemachineCamera poiCam) //to make the camera turn right
    {
        if (owner == null || poiCam == null || defaultVCam == null) return; //prevents null crash

        foreach (var s in _stack)
            if (s.owner == owner) return;

        var state = new POIState
        {
            owner = owner,
            poiCam = poiCam,
            prevDefaultPriority = defaultVCam.Priority,
            prevPoiPriority = poiCam.Priority
        };

        _stack.Add(state); //unity recognizing which POI is active
        poiCam.Priority = defaultVCam.Priority + activePriorityBoost; //camera turns right
    }

    public void Deactivate(Object owner)
    {
        int index = _stack.FindIndex(s => s.owner == owner);
        if (index < 0) return;

        bool wasTop = index == _stack.Count - 1;
        var removed = _stack[index];
        _stack.RemoveAt(index);
        //true = POI currently driving the view
        //false = another POI
        //tldr; swapping/interchangable POIs

        removed.poiCam.Priority = removed.prevPoiPriority;
        defaultVCam.Priority = removed.prevDefaultPriority;

        if (!wasTop) return;

        if (_stack.Count > 0)
        {
            var top = _stack[_stack.Count - 1];
            top.poiCam.Priority = defaultVCam.Priority + activePriorityBoost;
        }
    }
    public void JumpNext() //teleport controls keyboard + tablet? tablet to be continued
    {
        if (!HasValidPOIs()) return;

        EnsureIndexInitialized();

        int next = _currentIndex + 1;
        if (next >= poiCenters.Length)
            next = wrapAround ? 0 : poiCenters.Length - 1;

        _currentIndex = next;
        TeleportToIndex(_currentIndex);
    }

    public void JumpPrevious()
    {
        if (!HasValidPOIs()) return;

        EnsureIndexInitialized();

        int prev = _currentIndex - 1;
        if (prev < 0)
            prev = wrapAround ? poiCenters.Length - 1 : 0;

        _currentIndex = prev;
        TeleportToIndex(_currentIndex);
    }

    public void JumpToIndex(int index)
    {
        if (!HasValidPOIs()) return;

        _currentIndex = Mathf.Clamp(index, 0, poiCenters.Length - 1);
        _initializedIndex = true;
        TeleportToIndex(_currentIndex);
    }

    private void EnsureIndexInitialized()
        //TO BE UPDATE WORK IN PROGRESS, autopick nearest POI to current position
        //for now this just uses startindex (ascending + descending order)
    {
        if (_initializedIndex) return;

        _currentIndex = Mathf.Clamp(startIndex, 0, poiCenters.Length - 1);
        _initializedIndex = true;
    }

    private bool HasValidPOIs() //guard against missing entries, optional might remove
    {
        if (poiCenters == null || poiCenters.Length == 0) return false;

        for (int i = 0; i < poiCenters.Length; i++)
            if (poiCenters[i] == null)
                return false;

        return true;
    }

    private void TeleportToIndex(int index)
    {
        Transform target = poiCenters[index];
        TeleportTo(target);
    }

    private void TeleportTo(Transform target)
    {
        if (submarineRoot == null || target == null) return;//teleports the submarine

        if (submarineRb != null)
        {
            submarineRb.linearVelocity = Vector3.zero;
            submarineRb.angularVelocity = Vector3.zero;
            submarineRb.position = target.position;
            submarineRb.rotation = target.rotation;
        }
        else
        {
            submarineRoot.SetPositionAndRotation(target.position, target.rotation);
        }

        if (brain != null)
            brain.ManualUpdate();
    }
}

//tldr;
//manage which POI is active and teleports the submarine between POIs with X and Z on keyboard

//!!!!!!!!!   TO DO: NEED TO ADD CONFIGURATION FOR TABLET WITH X AND Z   !!!!!!!

//each POI has its own cinemachine camera
//when you enter a POI zone that camera becomes active
//when you leave it restores the previous one