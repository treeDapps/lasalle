using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class AugmentedReality : PanelUI, IEventListener<ScreenshotArgs>
                                       , IEventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    [SerializeField] GameObject instructions;
    void OnEnable()
    {
        ScreenshotEvent.RegisterListener(this);
        arEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        ScreenshotEvent.UnregisterListener(this);
        arEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        ScreenshotEvent.UnregisterListener(this);
        arEvent.UnregisterListener(this);
    }

    public void InstructionsOff()
    {
        instructions.SetActive(false);
        Debug.Log("Instructions off");
    }

    public void OnEventRaised(ScreenshotArgs arg0)
    {
        InstructionsOff();
    }

    public void OnEventRaised(ARArgs arg0)
    {
        Debug.Log("Ar listened " + arg0.arAction);
        if (arg0.arAction == "placedObject" || arg0.arAction == "back") InstructionsOff();
    }
}
