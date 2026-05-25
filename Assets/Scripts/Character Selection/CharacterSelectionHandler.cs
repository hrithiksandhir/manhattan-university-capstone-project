using UnityEngine; 
using UnityEngine.SceneManagement; 

public class CharacterSelectionHandler : MonoBehaviour 
{ 
    public CatCharacterManager catManager; 
    public DogCharacterManager dogManager; 
public void OnStartButtonPressed() 
{ 
    int catIndex = catManager.GetSelectedIndex(); 
    int dogIndex = dogManager.GetSelectedIndex(); 
    
    Debug.Log("🌟 Saving selected characters..."); 
    Debug.Log("🐱 Cat Index: " + catIndex); 
    Debug.Log("🐶 Dog Index: " + dogIndex); 
    
    PlayerPrefs.SetInt("SelectedCat", catIndex); 
    PlayerPrefs.SetInt("SelectedDog", dogIndex); 
    PlayerPrefs.Save(); 
    
    SceneManager.LoadScene("Levell01_Basement");

} 
public void BackToMainMenu()
    {
        SceneManager.LoadScene("modeSelection");
    }
} 
