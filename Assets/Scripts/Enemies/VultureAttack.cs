using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureAttack : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] private float health;
    [SerializeField] private GameObject deathSmoke;
    [SerializeField] private GameObject parent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Checks if the player is stomping
            Vector2 direction = transform.position - collision.transform.position;
            if (Vector2.Dot(direction.normalized, Vector2.down) > 0.25f)
            {
                Stomped(collision.gameObject);
            }

            else
            {
                // If colliding with the player, make the player take damage
                GameObject player = collision.gameObject;
                PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();

                playerHealth.TakeDamage(damage);

                // Set the vulture to the reset state so that it doesn't constantly chase the player
                VultureMovement vm = GetComponentInParent<VultureMovement>();
                vm.ResetState();
            }
        }
    }

    private void Stomped(GameObject player)
    {
        GameObject smoke = Instantiate(deathSmoke, gameObject.transform.position, Quaternion.identity);
        Destroy(smoke, 0.5f);
        player.GetComponent<PlayerMovement>().Jump();
        Destroy(parent);
    }
}
