using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed;
    public float resetOffset; // Distance to scroll before resetting

    private Vector2 startPosition;

    void Start()
    {
        startPosition = creditsText.anchoredPosition;
        StartCoroutine(ScrollCreditsLoop());
    }

    IEnumerator ScrollCreditsLoop()
    {
        while (true)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsText.anchoredPosition.y >= startPosition.y + resetOffset)
            {
                creditsText.anchoredPosition = startPosition;
            }

            yield return null;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
