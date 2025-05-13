using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public TypeResources type;
    public int id;
}

[System.Serializable]
public class Item : ItemBase
{
    public int value;
    public Item()
    {
        type = TypeResources.None;
        id = -1;
        value = 0;
    }
    
    public Item(string itemData)
    {
        var dataString = itemData.Split(':');
        var resourceName = dataString[0];
        type = (TypeResources)Enum.Parse(typeof(TypeResources), resourceName);
        if (type == TypeResources.Resources)
        {
            if (!int.TryParse(dataString[1], out id))
            {
                id = (int)(TypeResourcesValue)Enum.Parse(typeof(TypeResourcesValue), dataString[1]);
            }

        }
        else
        {
            id = int.Parse(dataString[1]);
        }

        try
        {
            value = int.Parse(dataString[2]);
        }
        catch (Exception)
        {
            value = 0;
        }
    }

    public Item(Item item)
    {
        this.type = item.type;
        this.id = item.id;
        this.value = item.value;
    }
    
    public Item(TypeResources type, int id, int value)
    {
        this.type = type;
        this.id = id;
        this.value = value;
    }
}