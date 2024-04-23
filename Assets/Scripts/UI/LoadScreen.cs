using AuraXR.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadScreen : EventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    [SerializeField] Animator animator;
    [SerializeField] Canvas canvas;
    [SerializeField] GraphicRaycaster raycaster;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        canvas.enabled = raycaster.enabled = false;
    }
    protected override void OnEnable()
    {
        arEvent.RegisterListener(this);
    }

    protected override void OnDisable()
    {
        arEvent.UnregisterListener(this);
    }

    public override void OnEventRaised(ARArgs data)
    {
        if (data.arAction.Equals("loadScreen"))
        {
            if(!data.boolean)
            {
                // prender el canvas y elevar el alpha al canvas group
                Debug.Log("quita loader");
                //animator.SetTrigger("out");
                //StartCoroutine(ToggleVisibility(false, 5));
                StartCoroutine(TurnOff());
            }
            else
            {
                // disminuir el alpha al canvas group y apagar el canvas
                StartCoroutine(ToggleVisibility(true, 0));
            }
        }
    }
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(3);

        animator.SetTrigger("out");

        yield return new WaitForSeconds(1);

        canvas.enabled = raycaster.enabled = false;
    }
    IEnumerator ToggleVisibility(bool b, int delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.enabled = raycaster.enabled = b;
    }
}
