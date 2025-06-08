using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Sources ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip backgroundMenu;
    public AudioClip backgroundGame;
    public AudioClip backgroundOver;
    public AudioClip win;
    public AudioClip die;
    public AudioClip lost;
    public AudioClip coin;
    public AudioClip jump;
    public AudioClip stomp;
    public AudioClip flag;
    public AudioClip pipe;
    public AudioClip dash;
    public AudioClip ending;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float pitchVariation = 0f)
    {
        if (pitchVariation > 0)
        {
            SFXSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        }
        else
        {
            SFXSource.pitch = 1f;
        }
        SFXSource.PlayOneShot(clip);
    }

    public void RestartMusic()
    {
        musicSource.Stop();
        musicSource.Play();
    }
}
