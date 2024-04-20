using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    public static PersistentObject Instance;

    private void Start()
    {
        if(Instance != null){
            //Debug.Log("Segun esta madre, ya existe este objeto: "+this.name+", lo destruiré");
            GameObject.Destroy(gameObject);
        }
        else{
            //Debug.Log("Pues no, no existia: "+this.name);
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

    }
}
