using System;
using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class Experiences : PanelUI, IEventListener<UIArgs>
                                  ,IPanel
{
    [SerializeField] BackgroundEvent background;
    [SerializeField] FaderEvent faderEvent;

    void OnEnable()
    {
        TurnOnPanelEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        TurnOnPanelEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        TurnOnPanelEvent.UnregisterListener(this);
    }

    public void GoForward(int newPanel)
    {
        faderEvent.Raise(new FaderArgs { faderTrigger= "out" });
        _sceneEvent.Raise(new SceneArgs { sceneAction = "ar", sceneIndex = 1 });
    }

    public void OnEventRaised(UIArgs arg0)
    {
        if (arg0.turnOnPanel == index && arg0.trigger != null)
        {
            //animator.SetTrigger(arg0.trigger);;
            if (arg0.trigger == "in")
            {
                Debug.Log("ahi va previous: "+previous+" turn on panel "+index);
                // Updates the previous and current indexes in the Back class
                //TurnOnPanelEvent.Raise(new UIArgs { uiAction = "ui", currentPanel = previous, turnOnPanel = index, debuglog = "vengo de Experiences" });
            }
        }
    }

    public IEnumerator corout_EnterAR(int newPanel)
    {
        yield return new WaitForSecondsRealtime(0.75f);
        TurnOnPanelEvent.Raise(new UIArgs { uiAction = "ui", turnOnPanel = newPanel, currentPanel = index, trigger = "in" });
    }
}
