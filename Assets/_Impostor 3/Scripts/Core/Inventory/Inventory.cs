using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<ItemResources> resources = new List<ItemResources>();

    public Inventory() { }
}
