using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class UIPanel : MonoBehaviour, IEventListener<UIArgs>
{
    [SerializeField] int panelIndex;
    [SerializeField] UIEvent turnOnPanelEvent;

    public UIEvent TurnOnPanelEvent => turnOnPanelEvent;
    public int PanelIndex => panelIndex;

    public Animator animator;
    [HideInInspector] public Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        turnOnPanelEvent.RegisterListener(this);
    }
    void OnDisable()
    {
        turnOnPanelEvent.UnregisterListener(this);
    }
    void OnDestroy()
    {
        turnOnPanelEvent.UnregisterListener(this);
    }
    public virtual void GoForward(int newPanel)
    {
        Debug.Log("go forward: " + newPanel);
        animator.SetTrigger("out");
        turnOnPanelEvent.Raise(new UIArgs { turnOnPanel = newPanel, currentPanel = panelIndex, trigger = "in" });
    }
    public virtual void OnEventRaised(UIArgs arg0)
    {
        Debug.Log(this.name+" ui event, turn on panel: " + arg0.turnOnPanel);
        if (arg0.turnOnPanel == panelIndex)
        {
            animator.SetTrigger(arg0.trigger);
        }
    }
}
