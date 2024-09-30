using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : CollectableItem
{
    protected override void Collect()
    {
        // Adds the item to the inventory
        Inventory inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        inventory.AddItem(gameObject);
    }
}
