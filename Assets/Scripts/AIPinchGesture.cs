
using UnityEngine;
using AuraXR.EventSystem;
using System.Collections.Generic;

public class AIPinchGesture : MonoBehaviour, IEventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    [SerializeField] Transform spawnedObject;

    float receivedScale;
    Vector2 initialTouchDistance;
    Vector3 initialScale;
    bool isScaling = false;
    bool isDragging = false;

    void OnEnable()
    {
        arEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        arEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        arEvent.UnregisterListener(this);
    }

    void Update()
    {
        if (spawnedObject == null) return;
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialTouchDistance = touch1.position - touch2.position;
                initialScale = spawnedObject.transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved
                && touch2.phase == TouchPhase.Moved
                && !isDragging)
            {
                Vector2 currentTouchDistance = touch1.position - touch2.position;
                float scaleFactor = currentTouchDistance.magnitude / initialTouchDistance.magnitude;

                var newScale = initialScale * scaleFactor;
                if (newScale.x > 0.15f && newScale.x < 1.5f)
                    spawnedObject.transform.localScale = initialScale * scaleFactor;
                if (!isScaling)
                {
                    isScaling = true;
                    arEvent.Raise(new ARArgs { arAction = "scaling" });
                }
            }
            else if (touch1.phase == TouchPhase.Moved
                && touch2.phase == TouchPhase.Moved
                && isDragging)
            {
                isDragging = false;
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                if (isScaling)
                {
                    isScaling = false;
                    arEvent.Raise(new ARArgs { arAction = "endScaling" });
                }
            }
        }
    }

    private void ReceiveData(Transform obj)
    {
        List<Transform> siblings = new List<Transform>(obj.GetComponentsInChildren<Transform>());
        Debug.Log("received: " + siblings[1].name);
        spawnedObject = obj;
        initialScale = spawnedObject.localScale;
    }

    public void OnEventRaised(ARArgs arg0)
    {
        var arAction = arg0.arAction;
        Debug.Log("ar action: " + arg0.arAction);
        if (arg0.arAction == "placedObject" || arg0.arAction == "newModel")
        {
            ReceiveData(arg0.spawnedObject);
        }
        else if (arAction.Equals("dragging"))
        {
            isDragging = true;
            StopAllCoroutines();
        }
        else if (arAction.Equals("endDragging"))
        {
            isDragging = false;
            StopAllCoroutines();
        }
    }
}
