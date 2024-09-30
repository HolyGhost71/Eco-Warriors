using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [SerializeField] GameObject keyNeeded;
    [SerializeField] string nextScene;

    [SerializeField] AudioClip unlockSFX;
    [SerializeField] AudioClip lockedSFX;

    // private bool isOpen = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.W))
            {
                // The key is not always needed for the door
                if (keyNeeded != null)
                {
                    // Check if the player has the correct key in the inventory
                    Inventory inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
                    if (inventory.ContainsItem(keyNeeded))
                    {
                        // Load the new scene
                        GameManager.Instance.PlayAudio(unlockSFX);
                        LoadScene();
                    }
                    else
                    {
                        GameManager.Instance.PlayAudio(lockedSFX);
                    }
                }

                else
                {
                    LoadScene();
                }
            }
        }
    }

    private void LoadScene()
    {
        GameManager.Instance.LoadScene(nextScene);
    }
}
