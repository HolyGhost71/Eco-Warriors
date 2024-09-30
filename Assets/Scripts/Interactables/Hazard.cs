using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float hazardDamage;
    [SerializeField] private bool instantKill = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Spikes hit!");

            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();

            // Some objects can be set to kill instantly, otherwise just take damage
            if (instantKill) playerHealth.Die();
            else playerHealth.TakeDamage(hazardDamage);
        }
    }
}
