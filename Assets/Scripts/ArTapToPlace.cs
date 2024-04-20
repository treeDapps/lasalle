using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ArTapToPlace : MonoBehaviour
{
    public GameObject goToInstantiate;
    private GameObject spawnedGo;
    private ARRaycastManager _arRaycastMan;
    private Vector2 touchPos;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public void Awake()
    {
        _arRaycastMan = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPos(out Vector2 touchPos)
    {
        if(Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }
        touchPos = default;
        return false;
    }

    public void Update()
    {
        if (!TryGetTouchPos(out Vector2 touchPosition))
            return;
        if(_arRaycastMan.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (spawnedGo == null)
                spawnedGo = Instantiate(goToInstantiate, hitPose.position, hitPose.rotation);
            else
                spawnedGo.transform.position = hitPose.position;
        }
    }
}
