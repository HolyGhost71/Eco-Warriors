using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rideable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            collision.transform.parent.SetParent(transform);
            GetComponent<Movement>().UpdateSpeed(3);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent.SetParent(null);
    }
}
