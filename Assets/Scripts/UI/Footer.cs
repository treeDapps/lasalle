using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class Footer : PanelUI, IEventListener<SceneArgs>
                            , IEventListener<ScreenshotArgs>
{

    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    void Start()
    {
        canvas.enabled = false;
        //buttons[1].SetActive(false);
        //buttons[0].SetActive(true);
    }
    void OnEnable()
    {
        //Debug.Log("menu on enable");
        _sceneEvent.RegisterListener(this);
        ScreenshotEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        _sceneEvent.UnregisterListener(this);
        ScreenshotEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        _sceneEvent.UnregisterListener(this);
        ScreenshotEvent.UnregisterListener(this);
    }

    public void OnEventRaised(SceneArgs arg0)
    {
        var loadedAR = arg0.sceneAction.Contains("arloaded");       
        var enabled = loadedAR ? true : false;
        canvas.enabled = enabled;
        
    }

    public void OnEventRaised(ScreenshotArgs arg0)
    {
        if(arg0.screenshotAction == "takescreenshot")
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
        }
        else if(arg0.screenshotAction == "endscreenshot")
        {
            buttons[1].SetActive(false);
            buttons[0].SetActive(true);
        }
    }
}
