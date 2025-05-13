using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class ItemInventory
{
    public TypeResources type;
    public ObscuredInt idItem;

    public abstract long Value { get; }

    public abstract void Add(long number);
    public abstract void Sub(long number);
}

[System.Serializable]
public class ItemResources : ItemInventory
{
    public ObscuredLong value;

    public override long Value
    {
        get
        {
            return value;
        }
    }

    public override void Add(long number)
    {
        value += number;
    }

    public override void Sub(long number)
    {
        value = (long)Mathf.Max(value - number, 0);
    }
}

[System.Serializable]
public abstract class ItemArtifact : ItemInventory
{
    public int idOnInventory;
    public int fragment;
    public ObscuredInt level = 1;

    public override long Value
    {
        get
        {
            return fragment;
        }
    }
    
    public virtual bool IsCanStack { get; private set; }

    public ItemArtifact()
    {

    }
}

[System.Serializable]
public class ItemStack : ItemArtifact
{
    public override bool IsCanStack
    {
        get
        {
            return true;
        }
    }

    public override void Add(long number)
    {
        fragment += (int)number;
    }

    public override void Sub(long number)
    {
        fragment -= (int)number;
    }

    public ItemStack()
    {
    }

}

[System.Serializable]
public class ItemDistinc : ItemArtifact
{
    public override bool IsCanStack
    {
        get
        {
            return false;
        }
    }
    
    public ItemDistinc() { }
    
    
    public override void Add(long number)
    {
        throw new NotImplementedException();
    }

    public override void Sub(long number)
    {
        throw new NotImplementedException();
    }
}
