using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpitScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask[] destroyableLayers;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);    
    }

    public void FlipSpit()
    {
        // Changes the diretion of the spit
        movementSpeed = -movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.CompareTag("Button"))
        {
            collision.gameObject.GetComponent<ButtonScript>().OnButtonPress();
        }
        
        Destroy(gameObject);
    }
}
