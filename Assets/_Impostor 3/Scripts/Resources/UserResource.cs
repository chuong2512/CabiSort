using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserResource
{
    public TypeResources type;

    public abstract void SetValue(object valueSet);
    public abstract void AddValue(int idAdd, long valueAdd);
    public abstract void SubValue(int idSub, long valueSub);
    public abstract long GetValue(int id);
    public void InvokeEventChange(int id, long value)
    {
        eventChangeValue?.Invoke(id, value);
    }
    public Action<int, long> eventChangeValue;
}

[System.Serializable]
public class UserResourceInventory<T> : UserResource where T : ItemInventory
{
    [SerializeField]
    protected List<T> itemCollections = new List<T>();
    public virtual List<T> CollectionsValue
    {
        get
        {
            return itemCollections;
        }
    }
    public override void AddValue(int idAdd, long valueAdd)
    {
        throw new NotImplementedException();
    }

    public override long GetValue(int id)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object valueSet)
    {
        throw new NotImplementedException();
    }

    public override void SubValue(int idSub, long valueSub)
    {
        throw new NotImplementedException();
    }
}


[System.Serializable]
public class UserResourceItem<T> : UserResourceInventory<T> where T : ItemResources
{
    public override void SetValue(object valueSet)
    {
        itemCollections = (List<T>)valueSet;
    }

    public override void AddValue(int idItem, long valueAdd)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (idItem == itemCollections[i].idItem)
            {
                itemCollections[i].Add(valueAdd);
                InvokeEventChange(idItem, valueAdd);
                return;
            }
        }

        var itemInstance = new ItemResources();
        itemInstance.idItem = idItem;
        itemInstance.value = valueAdd;
        itemCollections.Add(itemInstance as T);
        InvokeEventChange(idItem, valueAdd);
    }

    public override void SubValue(int id, long value)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == id)
            {
                itemCollections[i].Sub(value);
                InvokeEventChange(id, -value);
                return;
            }
        }
    }

    public T GetItemById(int id)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == id)
            {
                return itemCollections[i];
            }
        }
        return null;
    }

    public override long GetValue(int id)
    {
        var item = GetItemById(id);
        if (item != null)
        {
            return item.value;
        }
        else
        {
            return 0;
        }
    }

    public void AddOrOverrideValue(int id, long value)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == id)
            {
                itemCollections[i].value = value;
                InvokeEventChange(id, value);
                return;
            }
        }

        AddValue(id, value);
        //FruitSurvivalTracking.Instance.firebase.UpdateResource(((TypeResources)id).ToString(), ((TypeResourcesValue) id).ToString(), "Newbie", (int)value, TypeActionResouces.Earn);
    }
}

[System.Serializable]
public class UserResourceCollectionItem<T> : UserResourceInventory<T> where T : ItemArtifact
{
    public int limitStackCount = int.MaxValue;

    public override List<T> CollectionsValue
    {
        get
        {
            return itemCollections;
        }
    }

    private bool isCanStack = false;

    public override void SetValue(object valueSet)
    {
        itemCollections = (List<T>)valueSet;
        for (int i = 0; i < itemCollections.Count; i++)
        {
            itemCollections[i].type = this.type;
        }
        T item = System.Activator.CreateInstance<T>();
        isCanStack = item.IsCanStack;
    }

    public override void AddValue(int idItem, long valueAdd)
    {
        if (isCanStack)
        {
            for (int i = 0; i < itemCollections.Count; i++)
            {
                if (idItem == itemCollections[i].idItem)
                {
                    itemCollections[i].Add(valueAdd);
                    itemCollections[i].fragment = Mathf.Min(itemCollections[i].fragment, limitStackCount);
                    return;
                }
            }

            var itemInstance = new ItemStack();
            itemInstance.idItem = idItem;
            itemInstance.fragment = (int)valueAdd;
            itemCollections.Add(itemInstance as T);
        }
        //else
        //{
        //    var itemInstance = new ItemDistinc();
        //    itemInstance.idItem = idItem;
        //    itemInstance.fragment = 1;
        //    itemCollections.Add(itemInstance as T);
        //}
    }

    public override void SubValue(int idItem, long number)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == idItem)
            {
                if (itemCollections[i].IsCanStack)
                {
                    itemCollections[i].Sub(number);
                }
                return;
            }
        }
    }

    public T GetItemById(int id)
    {
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == id)
            {
                return itemCollections[i];
            }
        }
        return null;
    }

    public override long GetValue(int id)
    {
        //var item = GetItemById(id);
        //if (item == null)
        //{
        //    return 0;
        //}
        long count = 0;
        for (int i = 0; i < itemCollections.Count; i++)
        {
            if (itemCollections[i].idItem == id)
            {
                count += itemCollections[i].Value;
                if (isCanStack)
                {
                    return count;
                }
            }
        }
        return count;
    }
}
