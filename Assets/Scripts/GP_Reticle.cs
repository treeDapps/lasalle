using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GP_Reticle : MonoBehaviour
{
    [SerializeField] AREvent arEvent;
    public Transform reticle;
    public GameObject prefabToInstantiate;
    public UnityEvent setPrefabEvent;
    public UnityEvent foundGroundEvent;
    bool instantiated;
    bool hasGround = false;

    void FixedUpdate()
    {
        if (instantiated)
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

    private IEnumerator corout_SetPrefab()
    {
        GameObject instantiatedPrefab = GameObject.Instantiate(prefabToInstantiate);
        yield return new WaitUntil(() => instantiatedPrefab != null);
        instantiatedPrefab.transform.position = reticle.position;
        setPrefabEvent.Invoke();
        arEvent.Raise(new ARArgs { arAction = "placedObject", spawnedObject = instantiatedPrefab.transform });
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
}
