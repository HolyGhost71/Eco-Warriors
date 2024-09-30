using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float yOffset;

    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] bool followVert = true;

    void Update()
    {
        // Moves the camera based on the player's x co-ordinate
        // Clamps it between a certain point to stop it trailing off the edge
        Transform a = player.GetComponent<CharacterSwitcher>().GetAnimal().transform;

        if (followVert)
            transform.position = new Vector3(Mathf.Clamp(a.position.x, leftBound.position.x, rightBound.position.x), a.position.y + yOffset, -10);
        else
            transform.position = new Vector3(Mathf.Clamp(a.position.x, leftBound.position.x, rightBound.position.x), transform.position.y, -10);
    }
}
