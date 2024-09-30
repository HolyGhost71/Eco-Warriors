using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float movementSpeed = 0;
    [SerializeField] private LayerMask destroyableLayers;

    private float damage = 0;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * movementSpeed;
    }

    public void FlipBullet()
    {
        // Changes the diretion of the bullet
        movementSpeed = -movementSpeed;
    }

    public void SetVars(float _damage, float speed)
    {
        damage = _damage;
        movementSpeed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
