using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelathPickup : CollectableItem
{
    [SerializeField] private float health;

    protected override void Collect()
    {
        PlayerHealth player = GameObject.Find("Player").GetComponent<PlayerHealth>();
        player.AddHealth(health);
    }
}