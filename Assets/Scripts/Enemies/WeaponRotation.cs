using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    public void RotateWeapon()
    {
        Transform playerPosition = player.GetComponent<CharacterSwitcher>().GetAnimal().transform;

        // Rotate the gun to face the player
        Vector2 directionToPlayer = (playerPosition.position - gameObject.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector2.right, directionToPlayer);
        gameObject.transform.rotation = targetRotation;
    }
}
