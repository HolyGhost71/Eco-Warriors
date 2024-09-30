using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAttack : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject spitObject;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetTrigger("Attack");
        }
    }

    // Called on the exact frame of the animation
    public void OnAttack()
    {
        // Creates the spit object
        GameObject spit = Instantiate(spitObject, attackPoint.position, Quaternion.identity);
        
        // Sets the direction/rotation of the spit based on the direction of the player
        if (gameObject.transform.rotation.y == 0)
        {
            Debug.Log("Firing Right");
            spitObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Debug.Log("Firing Left");
            spitObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            spit.GetComponent<SpitScript>().FlipSpit();
        }
    }
}
