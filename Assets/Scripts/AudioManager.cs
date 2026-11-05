using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Clips")]
    public AudioClip music;
    public AudioClip run;
    public AudioClip jump;
    public AudioClip coin;
    public AudioClip gem;
    public AudioClip hit;
    public AudioClip gameOver;
    public AudioClip click;

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.35f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;
    [Range(0f, 1f)] public float runVolume = 0.4f;

    AudioSource musicSource, sfxSource, runSource;

    void Awake()
    {
        Instance = this;
        musicSource = NewSource(true, musicVolume);
        runSource = NewSource(true, runVolume);
        sfxSource = NewSource(false, 1f);
        musicSource.clip = music;
        runSource.clip = run;
    }

    void Start()
    {
        if (music != null) musicSource.Play();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    AudioSource NewSource(bool loop, float volume)
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.playOnAwake = false;
        s.loop = loop;
        s.volume = volume;
        s.spatialBlend = 0f;
        return s;
    }

    void PlayOnce(AudioClip c)
    {
        if (c != null) sfxSource.PlayOneShot(c, sfxVolume);
    }

    void SetRunning(bool on)
    {
        if (run == null) return;
        if (on && !runSource.isPlaying) runSource.Play();
        else if (!on && runSource.isPlaying) runSource.Stop();
    }

    // Static helpers so other scripts never crash when there is no AudioManager.
    public static void Coin() { if (Instance) Instance.PlayOnce(Instance.coin); }
    public static void Gem() { if (Instance) Instance.PlayOnce(Instance.gem); }
    public static void Jump() { if (Instance) Instance.PlayOnce(Instance.jump); }
    public static void Hit() { if (Instance) Instance.PlayOnce(Instance.hit); }
    public static void GameOverSound() { if (Instance) Instance.PlayOnce(Instance.gameOver); }
    public static void Click() { if (Instance) Instance.PlayOnce(Instance.click); }
    public static void Running(bool on) { if (Instance) Instance.SetRunning(on); }
}
