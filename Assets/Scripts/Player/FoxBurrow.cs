using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxBurrow : MonoBehaviour
{
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;
    private bool isCrouching = false;

    [SerializeField] LayerMask diggableGroundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        anim.SetBool("isCrouching", false);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isCrouching", true);
        }
    }

    // Called on the exact frame of the animation
    public void OnCrouch()
    {
        if (!isCrouching)
        {
            // Checks if the ground below the fox is diggable
            RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.3f, diggableGroundLayer);
            if (raycastHit.collider != null)
            {
                isCrouching = true;
                SuspiciousDirt ground = raycastHit.collider.gameObject.GetComponent<SuspiciousDirt>();
                ground.OnDug();
            }
        }
    }
}
