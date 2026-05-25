using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;
    public AudioClip mainMenuMusic; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        LoadVolume();
        audioSource.Play();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Levell01_Basement") 
        {
            StopMusic();
        }

        if (scene.name == "MainMenu" ||scene.name == "SettingsMenu" || scene.name == "CreditsScreen" || scene.name == "charcterSelection" || scene.name == "modeSelection") 
        {
            PlayMainMenuMusic(); 
        }
    }
    private void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void PlayMainMenuMusic()
    {
        if (audioSource != null)
        {
            if (audioSource.clip != mainMenuMusic)
            {
                audioSource.Stop();
                audioSource.clip = mainMenuMusic;
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.HasKey("VolumePreference") ? PlayerPrefs.GetFloat("VolumePreference") : 1f;
        bool isMuted = PlayerPrefs.HasKey("MutePreference") && PlayerPrefs.GetInt("MutePreference") == 1;

        audioSource.volume = isMuted ? 0f : savedVolume;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
