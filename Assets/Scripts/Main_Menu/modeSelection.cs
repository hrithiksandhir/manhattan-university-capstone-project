using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    public void PlaySolo()
    {
        PlayerPrefs.SetString("GameMode", "Solo");
        SceneManager.LoadScene("CharacterSelection");
    }

    public void PlayMultiplayer()
    {
        PlayerPrefs.SetString("GameMode", "Multiplayer");
        SceneManager.LoadScene("CharacterSelection");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
