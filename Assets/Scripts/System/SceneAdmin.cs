using System.Collections;
using AuraXR.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAdmin : MonoBehaviour, IEventListener<SceneArgs>
{
    [SerializeField] SceneEvent sceneEvent;
    string _sceneAction;

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
        if (arg0.sceneIndex > 0) StartCoroutine(corout_LoadScene(arg0));
    }

    IEnumerator corout_LoadScene(SceneArgs args)
    {
        _sceneAction = args.sceneAction;

        yield return new WaitForSeconds(1);

        var loaded = SceneManager.GetActiveScene();
        //Debug.Log("current loaded scene: " + loaded.buildIndex);
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
        var actionPerformed = _sceneAction + "loaded";
        var loaded = SceneManager.GetActiveScene();
        if(loaded.buildIndex > 0) sceneEvent?.Raise(new SceneArgs { sceneAction =  actionPerformed});
    }
}
