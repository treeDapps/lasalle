using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class Background : MonoBehaviour, IEventListener
                                       , IEventListener<BackgroundArgs>
                                       , IEventListener<SceneArgs>
{
    [SerializeField] BasicGameEvent switchBackground;
    [SerializeField] BackgroundEvent background;
    [SerializeField] SceneEvent sceneEvent;

    [SerializeField] Animator animator;
    Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    void OnEnable()
    {
        switchBackground.RegisterListener(this);
        background.RegisterListener(this);
        sceneEvent.RegisterListener(this);
    }
    void OnDisable()
    {
        switchBackground.UnregisterListener(this);
        background.UnregisterListener(this);
        sceneEvent.UnregisterListener(this);
    }
    void OnDestroy()
    {
        switchBackground.UnregisterListener(this);
        background.UnregisterListener(this);
        sceneEvent.UnregisterListener(this);
    }
    void SwitchBackground()
    {
        animator.SetTrigger("change");
    }

    public void OnEventRaised()
    {
        SwitchBackground();
    }

    public void OnEventRaised(BackgroundArgs arg0)
    {
        if (arg0.actionToPerform == "off") canvas.enabled = false;
        else if (arg0.actionToPerform == "on") canvas.enabled = true;
    }

    public void OnEventRaised(SceneArgs arg0)
    {
        if(arg0.sceneAction == "arloaded") canvas.enabled = false;
        else if (arg0.sceneAction == "uiloaded") canvas.enabled = true;
    }
}