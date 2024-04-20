using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShareScreenshot : MonoBehaviour
{
    [SerializeField] EventScreenshot screenshotEvent;
    private bool secure;
    private AudioSource obturizer;
    [SerializeField]
    private Image screenshot;
    [SerializeField]
    private static Image vrscreenshot;
    public static Image VRScreenshot
    {
        get => vrscreenshot;
        set => vrscreenshot = value;

    }
    [SerializeField]
    private string filePath;
    [SerializeField]
    private Texture2D screenshotTex;

    private bool vr;

    public delegate void ScreenshotEvent();
    public static ScreenshotEvent takeScreenshot;
    public static ScreenshotEvent vrTakeScreenshot;
    public static ScreenshotEvent showScreenshot;

    private void Awake()
    {
        obturizer = GetComponent<AudioSource>();
        screenshot = GetComponentInChildren<Image>();
    }

    public void OnEnable()
    {
        vrTakeScreenshot += VRTakeScreenshot;
    }

    public void OnDisable()
    {
        vrTakeScreenshot -= VRTakeScreenshot;
    }

    public void OnDestroy()
    {
        vrTakeScreenshot -= VRTakeScreenshot;
    }

    public void TakeScreenshot()
    {
        if (secure)
            return;
        secure = true;
        vr = false;
        StartCoroutine(corout_Screenshot());
    }

    public void VRTakeScreenshot()
    {
        if (secure)
            return;
        secure = true;
        vr = true;
        StartCoroutine(corout_Screenshot());
    }

    public void ScreenshotShare()
    {
        //NativeShare.(screenshot.sprite.texture, ShareApp.Whatsapp);
        //Menu.goBackwards();
        Debug.Log("File path: " + filePath);
        new NativeShare().AddFile(filePath).Share();
    }

    private IEnumerator corout_Screenshot()
    {
        //Debug.Log("Entro a rutina");
        //takeScreenshot?.Invoke();
        screenshotEvent.Raise(new ScreenshotArgs { screenshotAction = "takescreenshot" });
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(1);
        //EditorApplication.isPaused= true;
        filePath = Path.Combine(Application.persistentDataPath, "screenshot.jpg");
        Debug.Log(filePath);
        if (File.Exists(filePath))
            File.Delete(filePath);
        yield return new WaitForEndOfFrame();
#if UNITY_EDITOR
        ScreenCapture.CaptureScreenshot(filePath);
#else
        ScreenCapture.CaptureScreenshot("screenshot.jpg");
#endif
        yield return new WaitUntil(() => File.Exists(filePath) == true);
        byte[] FileData = File.ReadAllBytes(filePath);
        Texture2D tex2D = new Texture2D(2, 2);
        tex2D.LoadImage(FileData);
        yield return new WaitForEndOfFrame();
        screenshot.sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        yield return new WaitForEndOfFrame();
        //UnityEditor.EditorApplication.isPaused = true;
        takeScreenshot?.Invoke();
        yield return new WaitForEndOfFrame();
        //UnityEditor.EditorApplication.isPaused = true;
        showScreenshot?.Invoke();
        screenshotEvent.Raise(new ScreenshotArgs { screenshotAction = "showscreenshot" });

        secure = false;
    }

}
