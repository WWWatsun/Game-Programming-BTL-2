using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX Library")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip powerDownSound;

    [Header("Audio Settings")]
    [SerializeField] private float sfxVolume = 1f;

    // Singleton instance
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource; // dedicated audio source for one-shot SFX

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        // Ensure there's an AudioSource on this GameObject
        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.loop = false; // SFX should not loop
        sfxSource.volume = sfxVolume;
    }

    public void PlayShootSound()
    {
        PlaySFX(shootSound);
    }

    public void PlayExplosionSound()
    {
        PlaySFX(explosionSound);
    }

    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }

    public void PlayHurtSound()
    {
        PlaySFX(hurtSound);
    }

    public void PlayPowerUpSound()
    {
        PlaySFX(powerUpSound);
    }

    public void PlayPowerDownSound()
    {
        PlaySFX(powerDownSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        else if (clip == null)
        {
            Debug.LogWarning($"{nameof(AudioManager)}: Attempted to play a null audio clip.", this);
        }
    }
}
