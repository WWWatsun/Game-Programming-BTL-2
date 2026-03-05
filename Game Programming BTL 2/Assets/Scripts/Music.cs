using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    [Header("SFX Library")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip mainGameMusic;
    [SerializeField] private AudioClip victoryMusic;

    // Singleton instance
    public static Music Instance { get; private set; }

    private AudioSource musicSource; // dedicated music source

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true; // music loops
            musicSource.volume = 0.8f; // adjust as needed
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMenuMusic();
        }
        else
        {
            PlayMainGameMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }

    public void PlayMainGameMusic()
    {
        if (mainGameMusic != null)
        {
            musicSource.clip = mainGameMusic;
            musicSource.Play();
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic != null)
        {
            musicSource.clip = victoryMusic;
            musicSource.Play();
        }
    }
}
