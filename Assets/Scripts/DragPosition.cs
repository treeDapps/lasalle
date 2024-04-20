using System.Collections;
using AuraXR.EventSystem;
using UnityEngine;

public class DragPosition : MonoBehaviour, IEventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    Vector2 touchStartPos;
    Vector3 objectStartPos;
    bool isDragging = false;
    bool isScaling = false;

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
    public void OnEventRaised(ARArgs arg0)
    {
        var arAction = arg0.arAction;
        if (arAction.Equals("scaling"))
        {
            isScaling = true;
            isDragging = false;
            StopAllCoroutines();
        }
        else if (arAction.Equals("endScaling"))
        {
            isScaling = false;
            StopAllCoroutines();
        }
    }

    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
        objectStartPos = transform.position;
        isDragging = true;
        StartCoroutine(corout_StartDragging());
    }

    IEnumerator corout_StartDragging() 
    {
        yield return new WaitForSeconds(0.65f);
        isDragging = true;
        arEvent.Raise(new ARArgs { arAction = "dragging" });
    }

    void OnMouseDrag()
    {
        if (isDragging && !isScaling)
        {
            Vector2 touchDelta = (Vector2)Input.mousePosition - touchStartPos;
            Vector3 newPosition = objectStartPos + new Vector3(touchDelta.x, 0f, touchDelta.y) * 0.0015f;
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        StopAllCoroutines();
        arEvent.Raise(new ARArgs { arAction = "endDragging" });

    }
}
