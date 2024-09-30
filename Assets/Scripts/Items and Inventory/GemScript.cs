using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : CollectableItem
{ 
    protected override void Collect()
    {
        GameManager.Instance.AddGem();
    }
}