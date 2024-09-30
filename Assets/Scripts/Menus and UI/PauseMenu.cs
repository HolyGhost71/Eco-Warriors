using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        // Brings up the pause menu, pause the game and stop the timer
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.isTimerPaused = true;
        GameIsPaused = true;
    }

    public void Resume()
    {
        // Get rid of the pause menu, unpause the game and restart the timer
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.isTimerPaused = false;
        GameIsPaused = false;
    }

    public void MainMenu()
    {
        // Unpauses the game, loads the menu and clears the text
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
        GameManager.Instance.ClearText();
    }

    public void QuitGame()
    {
        // Quits
        Application.Quit();
    }
}
