using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : CollectableItem
{
    protected override void Collect()
    {
        GameManager.Instance.AddLife();
    }
}