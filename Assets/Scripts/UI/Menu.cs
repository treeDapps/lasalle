using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

public class Menu : PanelUI, IEventListener<FaderArgs>
{
    [SerializeField] FaderEvent faderEvent;


    void OnEnable()
    {
        faderEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        faderEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        faderEvent.UnregisterListener(this);
    }

    public void OnEventRaised(FaderArgs arg0)
    {
        if (arg0.faderTrigger == "crossfadedone") animator.SetTrigger("in");
    }
}

