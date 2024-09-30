using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignPost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] string textToDisplay;
    [SerializeField] float textDelay;

    private string currentlyDisplayingText = "";

    private Coroutine textAnimationCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the player is in the range, start showing the text
        if (collision.gameObject.CompareTag("Player"))
        {
            textAnimationCoroutine = StartCoroutine(AnimateText());
        }
    }

    private IEnumerator AnimateText()
    {
        // For each character, add it to the string and show the string
        // Between each one, wait a certain amount of time
        foreach (char c in textToDisplay)
        {
            currentlyDisplayingText += c;
            infoText.text = currentlyDisplayingText;
            yield return new WaitForSeconds(textDelay);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // When the player leaves the sign area, make sure to reset the string and stop the animation
        if (collision.gameObject.CompareTag("Player"))
        {
            if (textAnimationCoroutine != null)
            {
                StopCoroutine(textAnimationCoroutine);
            }
            currentlyDisplayingText = "";
            infoText.text = currentlyDisplayingText;
        }
    }
}
