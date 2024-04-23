using AuraXR.EventSystem;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Purchasing;

public class GP_Reticle : EventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    public Transform reticle;
    public GameObject prefabToInstantiate;
    public UnityEvent setPrefabEvent;
    public UnityEvent foundGroundEvent;
    public bool instantiated;
    public bool hasGround = false;
    public bool instantiateAssetBundle = true;
    public bool downloading = false;

    void Awake()
    {
        downloading = instantiateAssetBundle ? true : false;
        if(instantiateAssetBundle)
        {
            arEvent.Raise(new ARArgs
            {
                arAction = "loadScreen",
                boolean = true
            });
        }
    }
    void FixedUpdate()
    {
        if (instantiated || downloading)
            return;
        SetGPReticle();
        SetPrefab();
    }

    private void SetPrefab()
    {
        if (Input.GetMouseButtonDown(0) && hasGround)
        {
            instantiated = true;
            StartCoroutine(corout_SetPrefab());
        }
    }
    [Button]
    public void Test()
    {
        if (!instantiated)
        {
            instantiated = true;
            StartCoroutine(corout_SetPrefab());
        }
    }
    private IEnumerator corout_SetPrefab()
    {
        if (!instantiateAssetBundle)
        {
            GameObject instantiatedPrefab = GameObject.Instantiate(prefabToInstantiate);
            yield return new WaitUntil(() => instantiatedPrefab != null);
            instantiatedPrefab.transform.position = reticle.position;
            setPrefabEvent.Invoke();

            arEvent.Raise(new ARArgs
            {
                arAction = "placedObject",
                spawnedObject = instantiatedPrefab.transform
            });
        }
        else
        {
            setPrefabEvent.Invoke();
            arEvent.Raise(new ARArgs
            {
                arAction = "instantiateAssetBundle",
                position = reticle.position
            }); ;
        }
        Handheld.Vibrate();
        yield return new WaitForSeconds(1);
        reticle.gameObject.SetActive(false);
    }

    private void SetGPReticle()
    {
        RaycastHit hit = MakeRay();
        if (hit.collider != null)
        {
            reticle.localScale = Vector3.one;
            reticle.position = hit.point;
            reticle.eulerAngles = hit.normal;
            if (!hasGround)
            {
                hasGround = true;
                foundGroundEvent.Invoke();
            }
            //reticle.Rotate(new Vector3(90, 90, 90));
        }
        else
            SetReticleInSpace(hit.point);
    }

    private void SetReticleInSpace(Vector3 hitPoint)
    {
        reticle.localScale = Vector3.one;
        reticle.position = hitPoint;
    }

    private RaycastHit MakeRay()
    {
        RaycastHit hit;
        Vector3 ray = Camera.main.transform.forward;
        if (Physics.Raycast(this.transform.position, ray, out hit))
        {
            return hit;
        }
        else
            return hit;
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
            if(data.boolean == false)
            {
                Debug.Log("Start scanning floor!");
                downloading = false;
            }
        }
    }
}
