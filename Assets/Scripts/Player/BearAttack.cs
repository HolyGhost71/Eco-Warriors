using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask objectLayer;

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

    // Called on the specific frame of the attack animation
    public void OnAttack()
    {
        // Checks all of hit objects in the attack range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, objectLayer);

        foreach (Collider2D hitEnemy in hitObjects)
        {
            // Destroys blocks in the bear's way
            BreakableObject ob = hitEnemy.gameObject.GetComponent<BreakableObject>();
            ob.Break();
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
