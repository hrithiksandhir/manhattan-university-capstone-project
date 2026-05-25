using UnityEngine; 
using TMPro; 
public class CatCharacterManager : MonoBehaviour 
{ 
    public CharacterDatabase catDB; 
    public TMP_Text nameText; 
    public SpriteRenderer artworkSprite; 
    private int selectedOption = 0; 
void Start() 
{ 
    UpdateCharacter(); 
} 
public void NextOption() 
{ 
    selectedOption = (selectedOption + 1) % catDB.CharacterCount; 
    UpdateCharacter(); 
} 
public void BackOption() 
{ 
    if (selectedOption == 0) selectedOption = catDB.CharacterCount; 
    selectedOption--; 
    UpdateCharacter(); 
} 
private void UpdateCharacter() 
{ 
    Character character = catDB.GetCharacter(selectedOption); 
    artworkSprite.sprite = character.characterSprite; 
    nameText.text = character.characterName; 
} 
public int GetSelectedIndex() 
{ 
    return selectedOption; 
} 
} 