using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUI : MonoBehaviour
{
    [SerializeField] int panelIndex;
    [SerializeField] int previousIndex;
    [SerializeField] UIEvent turnOnPanelEvent;
    [SerializeField] SceneEvent sceneEvent;
    [SerializeField] EventScreenshot screenshotEvent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Canvas canvas;

    public int index => panelIndex;
    public UIEvent TurnOnPanelEvent => turnOnPanelEvent;
    public SceneEvent _sceneEvent => sceneEvent;
    public EventScreenshot ScreenshotEvent => screenshotEvent;
    [HideInInspector]public int previous;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
    }
}

public interface IPanel
{
    public void GoForward(int newPanel);
}
