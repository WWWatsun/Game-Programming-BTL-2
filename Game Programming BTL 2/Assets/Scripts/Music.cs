using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    [Header("Music Library")]
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
            
            // Initialize audio source once
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.volume = 0.8f;
            }

            // Register for scene load events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unregister from scene load events when destroyed
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        // Play appropriate music for the current scene
        PlayMusicForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Switch music whenever a scene loads
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
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
        if (menuMusic != null && musicSource != null)
        {
            musicSource.loop = true;
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }

    public void PlayMainGameMusic()
    {
        if (mainGameMusic != null && musicSource != null)
        {
            musicSource.loop = true;
            musicSource.clip = mainGameMusic;
            musicSource.Play();
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic != null && musicSource != null)
        {
            musicSource.loop = false; // victory music should not loop
            musicSource.clip = victoryMusic;
            musicSource.Play();
        }
    }
}
