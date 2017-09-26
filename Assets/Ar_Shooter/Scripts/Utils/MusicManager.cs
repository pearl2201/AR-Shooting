using UnityEngine;
using System.Collections;
using Dinosaur.Scripts;
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public enum MUSIC_TRACK
    {
        MENU,
        CAVE,
        DETAIL_GALLERY,
        FARM,
        NONE
    }
    public bool playOnStart = true;
    public enum CameraFollowMode
    {
        None,
        Follow,
        Child
    };
    public CameraFollowMode cameraFollowMode = CameraFollowMode.Follow;
    public int defaultTrack = 0;

    public bool initialVolumeFromPreference = true;
    public float _volume = 0.9f;
    public float _pitch = 1.0f;
    public bool ScaleOutputVolume = true;

    // Static singleton property
    public static MusicManager Instance { get; private set; }

    public bool isFading = false;
    public float volume
    {
        get { return _volume; }
        set
        {
            if (ScaleOutputVolume)
            {
                GetComponent<AudioSource>().volume = ScaleVolume(value);
            }
            else
            {
                GetComponent<AudioSource>().volume = value;
            }
            _volume = value;
        }
    }

    public float pitch
    {
        get { return _pitch; }
        set
        {
            GetComponent<AudioSource>().pitch = value;
            _pitch = value;
        }
    }

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
            return;
        }

        // Here we save our singleton instance
        Instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);


    }

    void Start()
    {
        if (cameraFollowMode == CameraFollowMode.Follow)
        {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        }
        else if (cameraFollowMode == CameraFollowMode.Child)
        {
            if (Camera.main != null)
                transform.parent = Camera.main.transform;
        }

        GetComponent<AudioSource>().clip = tracks[defaultTrack];
        volume = _volume;
        if (initialVolumeFromPreference)
            volume = Prefs.Instance.GetVolumeMusic();

        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.rolloffMode = AudioRolloffMode.Linear;
        audioSrc.loop = true;
        audioSrc.dopplerLevel = 0f;
        audioSrc.spatialBlend = 0f;
        if (playOnStart && volume != 0)
        {
            Play();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (cameraFollowMode == CameraFollowMode.Follow)
        {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        }
        else if (cameraFollowMode == CameraFollowMode.Child)
        {
            if (Camera.main != null)
                transform.parent = Camera.main.transform;
        }
    }

    void Update()
    {
        if (cameraFollowMode == CameraFollowMode.Follow)
        {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        }
    }

    public void PlayTrack(int i)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = tracks[i];
        GetComponent<AudioSource>().Play();
    }

    public void PlayTrack(MUSIC_TRACK track)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = tracks[(int) track];
        GetComponent<AudioSource>().Play();
    }

    public void Pause()
    {
        GetComponent<AudioSource>().Pause();
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void Fade(float targetVolume, float fadeTime)
    {
        LeanTween.value(gameObject, "SetScaleBaseVolume", volume, targetVolume, fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(FadeOutAsync(fadeTime));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAsync(0.3f));
    }
    IEnumerator FadeOutAsync(float fadeTime)
    {
        isFading = true;
        Fade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        Pause();
        isFading = false;
    }

    public void FadeIn(float fadeTime, MUSIC_TRACK newTrack)
    {
        StartCoroutine(IEFadeIn(fadeTime, newTrack));
    }

    public void FadeIn(MUSIC_TRACK newTrack)
    {
        StartCoroutine(IEFadeIn(0.3f, newTrack));
    }
    public void FadeIn(float fadeTime)
    {
        StartCoroutine(IEFadeIn(fadeTime, MUSIC_TRACK.NONE));
    }

    public IEnumerator IEFadeIn(float fadeTime, MUSIC_TRACK newTrack)
    {
        yield return new WaitWhile(() => (isFading == true));
        if (newTrack != MUSIC_TRACK.NONE)
        {
            GetComponent<AudioSource>().clip = tracks[(int)newTrack];
        }
        Play();
        Fade(1.0f, fadeTime);
        yield return new WaitForSeconds(1.0f);
        this.PostEvent(EventID.OnMusicChange);
    }

    public void ChangeTrack(MUSIC_TRACK newTrack)
    {
        StartCoroutine(IEChangeTrack(newTrack));
    }

    IEnumerator IEChangeTrack(MUSIC_TRACK newTrack)
    {
        float timeFade = 0.3f;

        FadeOut(timeFade);
        yield return new WaitForSeconds(timeFade + 0.1f);
        GetComponent<AudioSource>().clip = tracks[(int)newTrack];
        FadeIn(timeFade);
        yield return new WaitForSeconds(timeFade + 0.1f);
        this.PostEvent(EventID.OnMusicChange);

    }
    public void SlidePitch(float targetPitch, float fadeTime)
    {
        LeanTween.value(gameObject, "SetPitch", pitch, targetPitch, fadeTime);
    }

    public float GetVolumePreference()
    {
        return Prefs.Instance.GetVolumeMusic
            ();
    }

    public void SaveCurrentVolumePreference()
    {
        SaveVolumePreference(volume);
    }

    public void SaveVolumePreference(float v)
    {
        Prefs.Instance.SetVolumeMusic(v);
    }

    public void SetPitch(float p)
    {
        pitch = p;
    }

    public void SetScaleBaseVolume(float v)
    {
        GetComponent<AudioSource>().volume = volume * v;
        this.PostEvent(EventID.OnFadeMusic);
    }

    public float GetFadeVolume()
    {
        return GetComponent<AudioSource>().volume;
    }
    // TODO: we should consider using this dB scale as an option when porting these changes 
    //       over to unity-bowerbird: http://wiki.unity3d.com/index.php?title=Loudness
    /*
	 *   Quadratic scaling of actual volume used by AudioSource. Approximates the proper exponential.
	 */
    public float ScaleVolume(float v)
    {
        v = Mathf.Pow(v, 4);
        return Mathf.Clamp(v, 0f, 1f);
    }


}
