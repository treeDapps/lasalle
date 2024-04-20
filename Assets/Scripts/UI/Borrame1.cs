using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;


public class Borrame1 : MonoBehaviour, IEventListener<ScreenshotArgs>
{
    public int previous;
    public int current;
    public int next;

    public List<UI_Panel> panels = new List<UI_Panel>();
    public GameObject backButton;

    [SerializeField] SceneEvent sceneEvent;
    [SerializeField] FaderEvent faderEvent;
    [SerializeField] EventScreenshot screenshotEvent;
    [SerializeField] AREvent arEvent;

    void Start()
    {
        previous = -1;
        current = 0;
        next = 1;        
        panels[0].animator.SetTrigger("in");
        backButton.SetActive(false);
    }

    void OnEnable()
    {
        screenshotEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        screenshotEvent.UnregisterListener(this);
    }

    void OnDestroy()
    {
        screenshotEvent.UnregisterListener(this);
    }

    public void GoForward()
    {
        if (current == panels.Count -1) return;
        panels[current].animator.SetTrigger("out");
        panels[next].animator.SetTrigger("in");
        Animate("out", current);
        Animate("in", next);
        previous = panels[next].previous;
        current = panels[next].index;
        next = panels[next].next;
    }

    public void Forward(float delay)
    {
        if (current == panels.Count - 1) return;
        StartCoroutine(corout_(delay, next, "forward"));
    }

    public void Backward(float delay)
    {
        if (current == 0) return;
        StartCoroutine(corout_(delay, previous, "back"));
    }

    IEnumerator corout_(float delay, int inPanel, string direction)
    {
        backButton.SetActive(inPanel == 0 ? false : true);
        if (direction == "back" && inPanel == 1)
        {
            faderEvent.Raise(new FaderArgs { faderTrigger = "out" });
            sceneEvent.Raise(new SceneArgs { sceneAction = "ui", sceneIndex = 2 });
            delay = 1;
            arEvent.Raise(new ARArgs { arAction = "back"});
        }
        if (direction == "back" && inPanel == 2)
        {
            // debe mandar un evento a footer
            screenshotEvent.Raise(new ScreenshotArgs { screenshotAction = "endscreenshot" });
        }
        yield return new WaitForSeconds(delay);
        Animate("out", current);
        yield return new WaitForSeconds(0.5f);
        Animate("in", inPanel);
        current = panels[inPanel].index;
        if (direction == "forward")
        {
            previous = panels[inPanel].previous;
            next = panels[inPanel].next;
        }else if(direction == "back")
        {
            next = panels[inPanel].next;
            previous = panels[inPanel].previous;
        }
        
    }

    public void GoBack()
    {
        if (current == 0) return;
        Debug.Log("pone los valores de " + panels[previous].name);
        Animate("out", current);
        Animate("in", previous);
        current = panels[previous].index;
        next = panels[previous].next;
        previous = panels[previous].previous;
    }

    void Animate(string trigger, int panel)
    {
        foreach(var p in panels)
        {
            if(p.index == panel) p.animator.SetTrigger(trigger);    
        }
    }

    public void OnEventRaised(ScreenshotArgs arg0)
    {
        if (arg0.screenshotAction == "takescreenshot")
        {
            GoForward();
        }
    }
}
