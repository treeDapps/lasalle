using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;
using UnityEngine.Video;
using UnityEngine.UI;

public class MediaPlayer : MonoBehaviour, IEventListener<MediaPlayerArgs>
{
    public MediaPlayerEvent playClip;
    public GameEvent mediaHasFinished;

    public VideoClip[] clips;

    RawImage rawImage;

    public static VideoPlayer player;
    

    void Awake()
    {
        player = GetComponent<VideoPlayer>();
    }

    public void OnEventRaised(MediaPlayerArgs arg0)
    {
        throw new System.NotImplementedException();
    }

    void PlayVideo(int clip, RawImage rawImage, bool loop, bool fitScreenSize)
    {
        //Debug.Log("PLAY VIDEO");
        StartCoroutine(corout_PlayVideo(clip, rawImage, loop, fitScreenSize));
    }

    private IEnumerator corout_PlayVideo(int clip, RawImage customRawImage, bool loop, bool fitScreenSize)
    {
        player.Stop();
        yield return null;
        player.source = VideoSource.VideoClip;
        yield return new WaitForEndOfFrame();
        ///        Debug.Log(clips[clip].name+" w "+ (int)clips[clip].width + " h "+ (int)clips[clip].height);
        var rt = new RenderTexture((int)clips[clip].width, (int)clips[clip].height, 1);

        yield return new WaitUntil(() => rt != null);

        //Debug.Log("RT width: " + rt.width + ", height: " + rt.height);

        rawImage = customRawImage;
        RectTransform rect = rawImage.GetComponent<RectTransform>();
        if (fitScreenSize)
        {
            // Debug.Log("clip: "+clips[clip].name);
            var width = AspectRatio(new Vector2((int)clips[clip].width, (int)clips[clip].height)) * Screen.height;
            rect.sizeDelta = new Vector2(width, Screen.height);
        }
        //  Debug.Log("size delta" + rect.sizeDelta);
        rawImage.texture = rt;
        player.clip = clips[clip];
        player.targetTexture = rt;
        player.isLooping = loop;
        player.Play();
        //  Debug.Log("wait " + player.frameCount);
        yield return new WaitForSeconds((float)player.clip.length);
        // mediaHasFinished?.Invoke();
        mediaHasFinished?.Raise();
    }

    float AspectRatio(Vector2 size)
    {
        float aspectRatio = size.y < size.x ? size.y / size.x : size.x / size.y;
        //        Debug.Log("asp rat: " + aspectRatio);

        return (Mathf.Round(aspectRatio * 100)) / 100;
    }
}
