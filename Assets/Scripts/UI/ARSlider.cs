using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class ARSlider : MonoBehaviour, IEventListener<ScreenshotArgs>
                                     , IEventListener<ARArgs>
{
    [SerializeField] EventScreenshot screenshotEvent;
    [SerializeField] AREvent arEvent;
    [SerializeField] GameObject slider;
    Canvas canvas;
    void Awake() => canvas = GetComponent<Canvas>();
    void Start() => slider.gameObject.SetActive(false);

    void OnEnable()
    {
        screenshotEvent.RegisterListener(this);
        arEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        screenshotEvent.UnregisterListener(this);
        arEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        screenshotEvent.UnregisterListener(this);
        arEvent.UnregisterListener(this);
    }
    public void OnEventRaised(ScreenshotArgs arg0)
    {
        if (arg0.screenshotAction == "takescreenshot")
        {
            canvas.enabled = false;
        }
        else if (arg0.screenshotAction == "endscreenshot")
        {
            canvas.enabled = true;
        }
    }

    public void OnEventRaised(ARArgs arg0)
    {
        if (arg0.arAction == "placedObject") canvas.enabled = true;
        else if (arg0.arAction == "back") canvas.enabled = false;
    }
}
