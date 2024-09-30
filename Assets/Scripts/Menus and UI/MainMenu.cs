using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] gemTexts;
    [SerializeField] private TextMeshProUGUI[] timeTexts;

    private void Awake()
    {
        // For each level on the level select screen, retrieve and set the gem and time values
        for (int i = 1; i < gemTexts.Length + 1; i++)
        {
            string name = "GemsCollected" + i;
            gemTexts[i - 1].text = PlayerPrefs.GetInt(name).ToString();
            
            name = "TimeTaken" + i;
            timeTexts[i - 1].text = PlayerPrefs.GetFloat(name).ToString();
        }
    }

    public void LoadLevel(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
