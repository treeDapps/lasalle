using System.Collections;
using System.Threading;
using AuraXR.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAdmin : MonoBehaviour, IEventListener<SceneArgs>
{
    [SerializeField] SceneEvent sceneEvent;
    string _sceneAction;
    public int cont = 0;

    void OnEnable()
    {
        sceneEvent.RegisterListener(this);
        SceneManager.sceneLoaded += SceneHasLoaded;    
    }

    void OnDisable()
    {
        sceneEvent.UnregisterListener(this);
        SceneManager.sceneLoaded -= SceneHasLoaded;
    }

    void OnDestroy()
    {
        sceneEvent.UnregisterListener(this);
        SceneManager.sceneLoaded -= SceneHasLoaded;
    }

    public void OnEventRaised(SceneArgs arg0)
    {
        Debug.Log("==== scene to load: " + arg0.sceneIndex);
        if (arg0.sceneIndex > 0) StartCoroutine(corout_LoadScene(arg0));
    }

    public void LoadScene(SceneArgs args)
    {
        StartCoroutine(corout_LoadScene(args));
    }
    IEnumerator corout_LoadScene(SceneArgs args)
    {
        _sceneAction = args.sceneAction;
        Debug.Log("=== scene Admin "+_sceneAction);
        yield return new WaitForSeconds(1);

        var loaded = SceneManager.GetActiveScene();
        Debug.Log("current loaded scene: " + loaded.buildIndex);
        /* Uncomment for AR or GPAR experiences
        if (loaded.buildIndex.Equals(1))
            ARManager.arSession.Reset();
        else if (loaded.buildIndex.Equals(6))
            GPAR_Manager.arSession.Reset();
        */
        yield return new WaitForSeconds(1);

        if (args.sceneIndex == SceneManager.sceneCount - 1)
            Screen.orientation = ScreenOrientation.Portrait;
        else
            Screen.orientation = ScreenOrientation.AutoRotation;
        SceneManager.LoadScene(args.sceneIndex);
    }

    void SceneHasLoaded(Scene scene, LoadSceneMode mode)
    {
        if(cont == 0)
        {
            cont++;
            return;
        }
        cont++;
        var actionPerformed = _sceneAction + "loaded";
        var loaded = SceneManager.GetActiveScene();
        Debug.Log("===Scene has loaded. count "+cont+". Action performed: "+actionPerformed);
        if(loaded.buildIndex > 0) sceneEvent?.Raise(new SceneArgs { sceneAction =  actionPerformed});
    }
}
