using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    GameObject animal;

    [SerializeField] private AudioClip damageSFX;

    [SerializeField] private float playerMaxHealth;
    private float playerHealth;

    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        playerHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
    }

    public void TakeDamage(float damage)
    {
        // Finds the animal, changes it's health and plays the animation and SFX
        animal = GetComponent<CharacterSwitcher>().GetAnimal();
        playerHealth -= damage * animal.GetComponent<PlayerMovement>().animalParameters.health;
        animal.GetComponent<Animator>().SetTrigger("Hurt");
        GetComponent<AudioSource>().PlayOneShot(damageSFX);

        // Update the health bar
        healthBar.SetHealth(playerHealth);

        // Kill the player if necessary
        if (playerHealth < 0) { Die(); }
    }

    public void AddHealth(float health)
    {
        playerHealth += health;
        
        // Makes sure the health doesn't go over the max
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }

        healthBar.SetHealth(playerHealth);
    }

    public void Die()
    {
        GameObject activeAnimal = GetComponent<CharacterSwitcher>().GetAnimal();
        // anim.SetTrigger("Death");

        activeAnimal.GetComponent<PlayerMovement>().enabled = false;
        activeAnimal.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        activeAnimal.GetComponent<CapsuleCollider2D>().enabled = false;

        GameManager.Instance.Die();
    }
}
