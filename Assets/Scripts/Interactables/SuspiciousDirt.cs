using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciousDirt : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] bool isKey;

    public void OnDug()
    {
        // If the item is a key, simply move it from an external place to the correct spot then animate it moving
        // This means that it can still be assigned to the door, if the key was a cloned prefab it could not be connected
        // If it it any other item then it can be spawned there

        if (isKey)
        {
            item.transform.position = gameObject.transform.position;
        }
        else
        {
            item = Instantiate(item, gameObject.transform.position, Quaternion.identity);
        }

        item.GetComponent<CollectableItem>().StartCoroutine("AnimateItem"); ;
        gameObject.SetActive(false);
    }

    
}

