using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    public Animal animalParameters;

    private float moveSpeed;
    private float jumpPower;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D myCollider;

    private GameObject currentOneWayPlatform;

    float fallMultiplier = 5f;
    float lowJumpMultiplier = 8f;

    private AudioSource source;
    [SerializeField] AudioClip[] audioClips;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
        source = GetComponent<AudioSource>();

        // Sets the attributes based on the current animal
        moveSpeed = animalParameters.speed;
        jumpPower = animalParameters.jumpPower;
    }

    void Update()
    {
        // Moves the player
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Sets the animations
        anim.SetBool("isRunning", true);
        anim.SetFloat("y_velocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded());

        // Flips the player to face left/right and stay facing that way
        if (horizontalInput < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (horizontalInput > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else anim.SetBool("isRunning", false);

        // Checks to see if the player can jump based on whether they are touching the ground
        if (isGrounded())
        {
            // anim.SetBool("Grounded", true);
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

        // If the player is falling, speed up their falling momentum
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        
        // If the player is going up then decrease the gravity while they hold down space
        else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space)))
            rb.velocity += Vector2.up * Physics2D.gravity * lowJumpMultiplier * Time.deltaTime;

        // One Way Platform Behaviour
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentOneWayPlatform != null) StartCoroutine(DisableCollisions());
        }
    }

    // Allows the player to fall through the platform they are standing on
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform")) currentOneWayPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))  currentOneWayPlatform = null;
    }

    private IEnumerator DisableCollisions()
    {
        // Disables collisions with the platform for an appropriate amount of time
        TilemapCollider2D platformCollider = currentOneWayPlatform.GetComponent<TilemapCollider2D>();

        Physics2D.IgnoreCollision(myCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(myCollider, platformCollider, false);
    }

    private bool isGrounded()
    {
        // Raycasts downwards to see if the ground layer is below the player
        RaycastHit2D raycastHit = Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, 0.3f, groundLayer);
        return raycastHit.collider != null && raycastHit.normal == Vector2.up;
    }

    public void Jump()
    {
        // Adds a jump force
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private void PlayGrassStepSFX()
    {
        // For each step taken, play a random step sound effect from the array
        Random rand = new();
        int index = rand.Next(0, audioClips.Length - 1);

        source.PlayOneShot(audioClips[index]);
    }
}
