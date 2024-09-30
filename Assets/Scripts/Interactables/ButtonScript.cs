using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject[] destroyableObjects;
    [SerializeField] Sprite pressed;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnButtonPress()
    {
        foreach (var obj in destroyableObjects)
        {
            Destroy(obj);
        }

        sr.sprite = pressed;
    }
}
