using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AuraXR.EventSystem;

public class PinchGesture : MonoBehaviour, IEventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    private List<Touch> touches;
    public float delta;
    private float lastDelta;
    //private static Transform spawnedObject;
    public Transform spawnedObject;

    public float speedModifier;
    public Text speedText;
    public Vector3 distanciatotal;
    public GameObject imagenTouching;
    public Vector2 firstpose;
    public Vector2 lastpose;
    public Vector2 distancepose;
    public Vector3 pastpose;
    //public Text t;
    public Text percentageScale;
    public float inicialScale;
    public float totalPercentage;
    public int intPercentage;
    public bool dragON;
    public bool scaleON;

    private void OnEnable()
    {
        arEvent.RegisterListener(this);
        /*GPManager.objectHasBeenSpawned += ReceiveData;*/
    }

    private void OnDisable()
    {
        arEvent.UnregisterListener(this);
        /* GPManager.objectHasBeenSpawned -= ReceiveData;*/
    }

    private void OnDestroy()
    {
        arEvent.UnregisterListener(this);
        /*  GPManager.objectHasBeenSpawned -= ReceiveData;*/
    }

    private void ReceiveData(Transform obj)
    {
        spawnedObject = obj;
        inicialScale = spawnedObject.localScale.x;
    }

    public void Start()
    {
        touches = new List<Touch>();
        delta = 0;
        lastDelta = 0;
    }

    public void FixedUpdate()
    {
        if (spawnedObject == null)
            return;

        if (Input.touchCount == 1)
        {
            if (touches.Count == 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touches.Add(Input.GetTouch(0));
                }
            }

            if (!scaleON)
            {
                firstpose = Input.GetTouch(0).position;
                if (lastpose == Vector2.zero)
                {
                    pastpose = spawnedObject.localPosition;
                    lastpose = firstpose;
                }
                else
                {
                    dragON = true;
                    distancepose = (firstpose - lastpose) * speedModifier;
                    spawnedObject.localPosition = new Vector3(pastpose.x + distancepose.x, pastpose.y, pastpose.z + distancepose.y);
                    // speedText.text = "Posicion: " + spawnedObject.localPosition.ToString();
                }
            }

            /*if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                imagenTouching.SetActive(true);
                distanciatotal = 
                spawnedObject.localPosition += new Vector3(Input.GetTouch(0).position.x * speedModifier, spawnedObject.localPosition.y, Input.GetTouch(0).position.y * speedModifier);
                speedText.text = "Posicion: " + spawnedObject.localPosition.ToString();
            }*/

            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                dragON = false;
                lastpose = Vector2.zero;
                //imagenTouching.SetActive(false);
            }
        }
        if (Input.touchCount == 2)
        {
            if (touches.Count == 1)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    scaleON = true;
                    touches.Add(Input.GetTouch(1));
                }
            }
        }
        if (touches.Count == 2)
        {
            if (Input.touchCount == 2)
            {
                delta = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                //t.text = delta.ToString();
                if (lastDelta == 0)
                {
                    lastDelta = delta;
                }
                else
                {
                    var modelScale = spawnedObject.localScale;
                    var deltaScale = (Mathf.Abs(delta - lastDelta)) * 0.001f;
                    //Debug.Log("deltascale : " + deltaScale);
                    if (delta >= lastDelta)
                    {
                        modelScale = new Vector3(modelScale.x + deltaScale, modelScale.y + deltaScale, modelScale.z + deltaScale);
                    }
                    else
                    {
                        modelScale = new Vector3(modelScale.x - deltaScale, modelScale.y - deltaScale, modelScale.z - deltaScale);
                    }
                    lastDelta = delta;
                    spawnedObject.localScale = modelScale;
                    totalPercentage = (modelScale.x * 100) / inicialScale;
                    //Debug.Log("Total percentage: " + totalPercentage);
                    //intPercentage = (int)totalPercentage;
                    //percentageScale.text = intPercentage.ToString() + "%";
                    //percentageScale.color = new Color(255, 255, 255, 255);
                }
                if ((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
                {
                    scaleON = false;
                    lastDelta = 0;
                    delta = 0;
                    touches.Remove(Input.GetTouch(1));
                    touches.Remove(Input.GetTouch(0));
                    //percentageScale.color = new Color(255, 255, 255, 0);
                }
                if ((Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled))
                {
                    scaleON = false;
                    lastDelta = 0;
                    delta = 0;
                    touches.Remove(Input.GetTouch(1));
                    //percentageScale.color = new Color(255, 255, 255, 0);
                }
            }
        }

    }

    public void OnEventRaised(ARArgs arg0)
    {
        if (arg0.arAction == "placedObject") ReceiveData(arg0.spawnedObject);
    }
}
