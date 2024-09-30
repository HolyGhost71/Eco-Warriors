using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum State { Patrol, Attack, Reset };

public class VultureMovement : MonoBehaviour
{
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform resetPoint;
    private Transform nextPoint;

    [SerializeField] float speed;

    private State state = State.Patrol;

    private CircleCollider2D circleCollider;

    [SerializeField] GameObject player;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackSound;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        nextPoint = rightPoint;
    }

    void Update()
    {
        // step represents the distance the vulture should cover every frame
        float step = speed * Time.deltaTime;

        // While in state patrol, move towards the right/left point, when it reaches there, switch it's goal to the left/right point
        if (state == State.Patrol)
        {
            if (transform.position == nextPoint.position)
            {
                if (nextPoint == leftPoint) nextPoint = rightPoint;
                else if (nextPoint == rightPoint) nextPoint = leftPoint;
            }

            // Flips the vulture to face where it is moving
            if (transform.position.x < nextPoint.position.x) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.position.x > nextPoint.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);

            transform.position = Vector2.MoveTowards(transform.position, nextPoint.position, step);
        }

        // While in state attack, move towards the player
        else if (state == State.Attack)
        {
            Vector3 playerPosition = player.GetComponent<CharacterSwitcher>().GetAnimal().transform.position;

            if (transform.position.x < playerPosition.x) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.position.x > playerPosition.x) transform.rotation = Quaternion.Euler(0, 180, 0);

            // Flips the vulture to face the player
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, step);
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


    public void ResetState()
    {
        state = State.Reset;
        StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        // Stops the vulture from being able to lock back onto the player while it is resetting - allows the player to escape once hit
        Physics2D.IgnoreCollision(circleCollider, player.GetComponent<CharacterSwitcher>().GetAnimal().GetComponent<CapsuleCollider2D>());
        yield return new WaitForSeconds(3f);
        Physics2D.IgnoreCollision(circleCollider, player.GetComponent<CharacterSwitcher>().GetAnimal().GetComponent<CapsuleCollider2D>(), false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters the range, switch the vulture to attacking
        if (collision.gameObject == player.GetComponent<CharacterSwitcher>().GetAnimal())
        {
            GameObject animal = player.GetComponent<CharacterSwitcher>().GetAnimal();

            // Make sure the vulture is facing the player - can only attack if it sees
            if ((animal.transform.position.x < transform.position.x) && (nextPoint = leftPoint)
                || (animal.transform.position.x > transform.position.x) && (nextPoint = rightPoint))
            {
                state = State.Attack;
                // audioSource.PlayOneShot(attackSound);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the player leaves the range, reset
        if (collision.gameObject == player.GetComponent<CharacterSwitcher>().GetAnimal())
        {
            ResetState();
        }
    }
}
