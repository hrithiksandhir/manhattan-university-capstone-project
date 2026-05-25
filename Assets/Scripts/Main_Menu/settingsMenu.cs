using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;
    private float savedVolume = 1f;
    private bool isLoading = true;
    public GameObject controlMap;


    void Start()
    {
        LoadSettings();
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(Mute);
        isLoading = false;
    }

    public void SetVolume(float volume)
    {
        if (!isLoading && !muteToggle.isOn)
        {
            savedVolume = volume;
            AudioListener.volume = volume;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(volume);
            }
        }

        SaveSettings();
    }

    public void Mute(bool isMuted)
    {
        if (isLoading) return;

        if (isMuted)
        {
            savedVolume = volumeSlider.value;
            AudioListener.volume = 0f;
            volumeSlider.interactable = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(0f);
            }
        }
        else
        {
            AudioListener.volume = savedVolume;
            volumeSlider.interactable = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(savedVolume);
            }
        }

        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePreference", savedVolume);
        PlayerPrefs.SetInt("MutePreference", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        savedVolume = PlayerPrefs.HasKey("VolumePreference") ? PlayerPrefs.GetFloat("VolumePreference") : 1f;
        bool isMuted = PlayerPrefs.HasKey("MutePreference") && PlayerPrefs.GetInt("MutePreference") == 1;

        muteToggle.onValueChanged.RemoveListener(Mute);
        muteToggle.isOn = isMuted;
        muteToggle.onValueChanged.AddListener(Mute);

        volumeSlider.value = savedVolume;
        volumeSlider.interactable = !isMuted;

        AudioListener.volume = isMuted ? 0f : savedVolume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(isMuted ? 0f : savedVolume);
        }
    }

    public void ToggleControlMap()
    {
        bool isActive = controlMap.activeSelf;
        controlMap.SetActive(!isActive);
    }

    public void BackToMainMenu()
    {
        SaveSettings();
        StartCoroutine(LoadMainMenuAfterSave());
    }

    public void BackToSettingsMenu()
    {
        SaveSettings();
        StartCoroutine(LoadSettingsMenuAfterSave());
    }
    private IEnumerator LoadMainMenuAfterSave()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator LoadSettingsMenuAfterSave()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("SettingsMenu");
    }
}
