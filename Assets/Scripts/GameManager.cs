using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int gemsCollected = 0;
    [SerializeField] private int lives = 5;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI gemsText;

    [SerializeField] CanvasGroup fadeImage;

    private AudioSource audioSource;
    [SerializeField] AudioClip deathSFX;

    private float timeElapsed = 0f;
    public bool isTimerPaused = false;

    // Singleton instance
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Find existing GameManager instance in the scene
                instance = FindObjectOfType<GameManager>();

                // If no instance exists, create a new one
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // When the level is loaded, run up the timer
        if (!isTimerPaused)
            timeElapsed += Time.deltaTime;
    }

    public void AddGem()
    {
        // Update the number of gems and change the UI
        gemsCollected++;
        gemsText.text = "Gems: " + gemsCollected;
    }

    public void AddLife()
    {
        // Update the number of lives and change the UI
        lives++;
        livesText.text = "Lives: " + lives;
    }

    public void Die()
    {
        // Decrease the lives
        lives--;
        PlayAudio(deathSFX);

        // If the player has ran out of lives, go back to the menu
        if (lives == 0)
        {
            lives = 5;
            StartCoroutine(FadeOutAndLoad("StartMenu"));
        }

        else
        {
            gemsCollected = 0;
            StartCoroutine(FadeOutAndLoad(SceneManager.GetActiveScene().name));
        }
    }

    private IEnumerator FadeOutAndLoad(String scene)
    {
        Debug.Log("Start fade");

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            fadeImage.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration); ;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        LoadScene(scene);

        yield return new WaitForSeconds(0.5f);

        fadeImage.alpha = 0f;
    }

    public void LoadScene(string sceneName)
    {
        // As long as the same scene isn't reloading (i.e. death), save the data
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            SaveData();
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Reset the tracked variables
        timeElapsed = 0f;
        gemsCollected = 0;

        // Reset the UI
        if (sceneName != "StartMenu")
        {
            gemsText.text = "Gems: " + gemsCollected;
            livesText.text = "Lives: " + lives;
        }

        // If going back to the menu, get rid of the text
        else
        {
            ClearText();
        }
    }

    public void ClearText()
    {
        gemsText.text = "";
        livesText.text = "";
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void SaveData()
    {
        int sceneNo = SceneManager.GetActiveScene().buildIndex;

        // Saves the number of gems for that level if it beats the previous score
        string name = "GemsCollected" + sceneNo;
        if (gemsCollected > PlayerPrefs.GetInt(name))
            PlayerPrefs.SetInt(name, gemsCollected);

        // Saves the best time for the level - or adds it if there is no best time
        name = "TimeTaken" + sceneNo;
        if (timeElapsed < PlayerPrefs.GetFloat(name) || PlayerPrefs.GetFloat(name) == 0)
            PlayerPrefs.SetFloat(name, timeElapsed);
    }
}
