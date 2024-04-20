using UnityEngine;
using AuraXR.EventSystem;
using UnityEngine.Video;
using UnityEngine.UI;

public enum SplashType
{
    Image,
    Video
}

public class Splash : MonoBehaviour, IEventListener<FXArgs>, IEventListener<FaderArgs>
{
    [SerializeField] MediaPlayerEvent playVideoclip;

    [SerializeField] FaderEvent faderEvent;

    [SerializeField] FXEvent playAudioclip;

    [SerializeField] SplashType type;
    [SerializeField] AudioClip splashAudioclip;
    [SerializeField] VideoClip splashClip;

    [SerializeField] RawImage rawImage;
    [SerializeField] Image staticImage;

    void Awake()
    {
        if (type.Equals(SplashType.Video))
        {
            rawImage.gameObject.SetActive(true);
            staticImage.gameObject.SetActive(false);
        }
        else if (type.Equals(SplashType.Image))
        {
            rawImage.gameObject.SetActive(false);
            staticImage.gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        playAudioclip.RegisterListener(this);
        faderEvent.RegisterListener(this);
    }
    void OnDisable()
    {
        playAudioclip.UnregisterListener(this);
        faderEvent.UnregisterListener(this);
    }
    void OnDestroy()
    {
        playAudioclip.UnregisterListener(this);
        faderEvent.UnregisterListener(this);
    }

    void Start()
    {
        if (splashClip == null && rawImage == null && staticImage == null)
            DestroySplash();
        else
            PlaySplash();
    }

    private void PlaySplash()
    {
        if (type.Equals(SplashType.Video))
        {
            playVideoclip.Raise(new MediaPlayerArgs { clip = 0, rawImage = rawImage, loop = false, fitScreenSize = true });
        }
        else if (type.Equals(SplashType.Image))
        {
            playAudioclip.Raise(new FXArgs { type = "audioclip", clip = splashAudioclip });
        }
    }
    private void DestroySplash()
    {
        Destroy(this.gameObject, 0);
    }

    public void OnEventRaised(FXArgs arg0)
    {
        if (arg0.finished)
        {
            faderEvent.Raise(new FaderArgs { faderTrigger = "crossfade" });
        }
    }

    public void OnEventRaised(FaderArgs arg0)
    {
        if (arg0.faderTrigger == "crossfadedone") DestroySplash();
    }
    


}
