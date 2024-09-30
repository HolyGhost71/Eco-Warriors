using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject containedObject;
    [SerializeField] AudioSource breakSFX;
    public void Break()
    {
        // Place the item in the spot if necessary
        if (containedObject != null)
        {
            Instantiate(containedObject, gameObject.transform.position, Quaternion.identity);
        }

        // Destroy the box
        breakSFX.Play();
        Destroy(gameObject);
    }

}
