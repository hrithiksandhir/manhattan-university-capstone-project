using UnityEngine; 
using TMPro; 
public class DogCharacterManager : MonoBehaviour 
{ 
    public CharacterDatabase dogDB; 
    public TMP_Text nameText; 
    public SpriteRenderer artworkSprite; 
    private int selectedOption = 0; 
    
    void Start() 
    { 
        UpdateCharacter(); 
    } 
    public void NextOption() 
    { 
        selectedOption = (selectedOption + 1) % dogDB.CharacterCount; 
        UpdateCharacter(); 
    } 
    public void BackOption() 
    { 
        if (selectedOption == 0) selectedOption = dogDB.CharacterCount; 
        selectedOption--; 
        UpdateCharacter(); 
    } 
    
    private void UpdateCharacter() 
    { 
        Character character = dogDB.GetCharacter(selectedOption); 
        artworkSprite.sprite = character.characterSprite; 
        nameText.text = character.characterName; 
    } 
    public int GetSelectedIndex() 
    { 
        return selectedOption; 
        } 
} 