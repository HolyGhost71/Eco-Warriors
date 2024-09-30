using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Stores a list of all the items in the inventory
    public List<GameObject> itemList = new();
    public void AddItem(GameObject item)
    {
        itemList.Add(item);
        // PrintItems();
    }
    public void RemoveItem(GameObject item)
    {
        itemList.Remove(item);
    }

    public void PrintItems()
    {
        foreach (GameObject item in itemList)
        {
            Debug.Log(item);
        }
    }

    public bool ContainsItem(GameObject item)
    {
        foreach (GameObject i in itemList)
        {
            if (i.name == item.name) return true;
        }
        return false;
    }
}