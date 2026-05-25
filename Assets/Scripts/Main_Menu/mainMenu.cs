using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // The references for each panel (assigned in the Unity Inspector)
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject playGameMenu;

    void Start()
    {
        // Log initialization to ensure Start() runs only once
        Debug.Log("MainMenu Start() called");

        // Show the main menu with all buttons visible
        mainMenu.SetActive(true);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(true);
        playGameMenu.SetActive(true);
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame() called");
        SceneManager.LoadScene("modeSelection");

    }

    // Method to open the Settings menu
    public void OpenSettings()
    {
        Debug.Log("OpenSettings() called");
        SceneManager.LoadScene("SettingsMenu");
    }

    // Method to open the Credits menu
    public void OpenCredits()
    {
        Debug.Log("OpenCredits() called");
        SceneManager.LoadScene("CreditsScreen");
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("QUIT!"); // Debug message
        Application.Quit(); // Quit the game
    }
}

