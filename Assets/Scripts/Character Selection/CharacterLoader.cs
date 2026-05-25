using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public CharacterDatabase catDB;
    public CharacterDatabase dogDB;
    public GameObject catObject;
    public GameObject dogObject;

    void Start()
    {
        Debug.Log("🚀 CharacterLoader: Start called");

        int selectedCat = PlayerPrefs.GetInt("SelectedCat", 0);
        int selectedDog = PlayerPrefs.GetInt("SelectedDog", 0);

        Debug.Log("🎯 Selected Cat Index: " + selectedCat);
        Debug.Log("🎯 Selected Dog Index: " + selectedDog);

        Character catChar = catDB.GetCharacter(selectedCat);
        Character dogChar = dogDB.GetCharacter(selectedDog);

        // --- CAT ---
        Transform catSpriteTransform = catObject.transform.Find("CatSprite");
        if (catSpriteTransform != null)
        {
            SpriteRenderer catRenderer = catSpriteTransform.GetComponent<SpriteRenderer>();
            Animator catAnimator = catSpriteTransform.GetComponent<Animator>();

            if (catRenderer != null && catChar != null)
            {
                catRenderer.sprite = catChar.characterSprite;
                Debug.Log("✅ Cat sprite updated to: " + catChar.characterName);
            }
            else
            {
                Debug.LogWarning("⚠️ Cat SpriteRenderer or Character is null");
            }

            if (catAnimator != null && catChar.characterAnimator != null)
            {
                catAnimator.runtimeAnimatorController = catChar.characterAnimator;
                Debug.Log("✅ Cat animator assigned: " + catChar.characterAnimator.name);
            }
            else
            {
                Debug.LogWarning("⚠️ Cat Animator or Controller is missing.");
            }
        }
        else
        {
            Debug.LogError("❌ Could not find CatSprite child in Cat object");
        }

        // --- DOG ---
        Transform dogSpriteTransform = dogObject.transform.Find("DogSprite");
        if (dogSpriteTransform != null)
        {
            SpriteRenderer dogRenderer = dogSpriteTransform.GetComponent<SpriteRenderer>();
            Animator dogAnimator = dogSpriteTransform.GetComponent<Animator>();

            if (dogRenderer != null && dogChar != null)
            {
                dogRenderer.sprite = dogChar.characterSprite;
                Debug.Log("✅ Dog sprite updated to: " + dogChar.characterName);
            }
            else
            {
                Debug.LogWarning("⚠️ Dog SpriteRenderer or Character is null");
            }

            if (dogAnimator != null && dogChar.characterAnimator != null)
            {
                dogAnimator.runtimeAnimatorController = dogChar.characterAnimator;
                Debug.Log("✅ Dog animator assigned: " + dogChar.characterAnimator.name);
            }
            else
            {
                Debug.LogWarning("⚠️ Dog Animator or Controller is missing.");
            }
        }
        else
        {
            Debug.LogError("❌ Could not find DogSprite child in Dog object");
        }
    }
}