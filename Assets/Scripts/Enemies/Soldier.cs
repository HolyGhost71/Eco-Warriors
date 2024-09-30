using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform resetPoint;
    [SerializeField] float speed;
    private Transform nextPoint;
    private State state = State.Patrol;

    [Header("Attacking")]
    [SerializeField] float attackCooldown;
    private float attackTimer;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletDamage;

    private Animator anim;

    [SerializeField] GameObject player;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackSound;

    [Header("Health and Damage")]
    [SerializeField] private float health;
    [SerializeField] private GameObject deathSmoke;
    [SerializeField] private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        nextPoint = rightPoint;
        attackTimer = attackCooldown;
    }

    void Update()
    {
        // step represents the distance the soldier should cover every frame
        float step = speed * Time.deltaTime;
        anim.SetBool("isWalking", true);

        // While in state patrol, move towards the right/left point, when it reaches there, switch it's goal to the left/right point
        if (state == State.Patrol)
        {
            if (transform.position == nextPoint.position)
            {
                if (nextPoint == leftPoint) nextPoint = rightPoint;
                else if (nextPoint == rightPoint) nextPoint = leftPoint;
            }

            // Flips the soldier to face where it is moving
            if (transform.position.x < nextPoint.position.x) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.position.x > nextPoint.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);

            transform.position = Vector2.MoveTowards(transform.position, nextPoint.position, step);
        }

        // While in state attack, move towards the player
        else if (state == State.Attack)
        {
            Transform playerPosition = player.GetComponent<CharacterSwitcher>().GetAnimal().transform;
            anim.SetBool("isWalking", false);

            if (attackTimer >= attackCooldown)
            {
                // Set the direction for where the bullet will shoot
                Vector2 direction = (playerPosition.position - firePoint.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Rotates the bullet sprite
                GameObject b = Instantiate(bullet, firePoint.position, Quaternion.Euler(0f, 0f, angle));
                b.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                b.GetComponent<BulletScript>().SetVars(bulletDamage, bulletSpeed);

                // Create the muzzle flash
                StartCoroutine(MuzzleFlash());
                audioSource.PlayOneShot(attackSound);

                Destroy(b, 5f);
                attackTimer = 0;
            }
            else attackTimer += Time.deltaTime;
        }

        // When in the reset state, head back to the reset point that has been assigned
        else if (state == State.Reset)
        {
            transform.position = Vector2.MoveTowards(transform.position, resetPoint.position, step);
            if (transform.position == resetPoint.position)
            {
                state = State.Patrol;
            }

            // Flips the vulture to face where it is moving
            if (transform.position.x < resetPoint.position.x) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.position.x > resetPoint.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        muzzleFlash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters the range, switch the soldier to attacking
        if (collision.gameObject == player.GetComponent<CharacterSwitcher>().GetAnimal())
        {
            GameObject animal = player.GetComponent<CharacterSwitcher>().GetAnimal();

            // Make sure the soldier is facing the player - can only attack if it sees
            if ((animal.transform.position.x < transform.position.x) && (nextPoint = leftPoint)
                || (animal.transform.position.x > transform.position.x) && (nextPoint = rightPoint))
            {
                state = State.Attack;
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the player leaves the range, reset
        if (collision.gameObject == player.GetComponent<CharacterSwitcher>().GetAnimal())
        {
            state = State.Reset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Checks if the player is stomping
            Vector2 direction = transform.position - collision.transform.position;
            if (Vector2.Dot(direction.normalized, Vector2.down) > 0.9f)
            {
                Stomped(collision.gameObject);
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
