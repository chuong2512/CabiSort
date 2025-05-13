using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class ResourcesUtil : SingletonMonoDontDestroy<ResourcesUtil>
{
    private Dictionary<TypeResources, UserResource> resourceDict = new Dictionary<TypeResources, UserResource>();

    public bool CheckResource(TypeResources type, int id, long value)
    {
        if (type == TypeResources.None)
        {
            return true;
        }
        else
        {
            long resource = GetResource<UserResource>(type).GetValue(id);
            return resource >= value;
        }
    }
    
    public bool CheckResource(Item item)
    {
        return CheckResource(item.type, item.id, item.value);
    }
    
    public void AddResource(List<Item> items, string source = "", bool isBuy = false, bool isSave = true)
    {
        foreach (var item in items)
        {
            AddResource(item.type, item.id, item.value,source, isBuy, isSave);
        }
    }
    
    public void AddResource(Item item, string source = "", bool isBuy = false, bool isSave = true)
    {
        AddResource(item.type, item.id, item.value, source, isBuy, isSave);
    }
    
    
    public void AddResource(TypeResources resourcesType, int id, long value, string source = "", bool isBuy = false, bool isSave = true)
    {
        AddResource(resourcesType, id, value, true);
        // if (!source.Equals(ItemSources.PaidApp))
        // {
        if (!isBuy)
        {
            //FruitSurvivalTracking.Instance.firebase.UpdateResource(resourcesType.ToString(), ((TypeResourcesValue) id).ToString(), source , (int)value, TypeActionResouces.Earn);
        }
        else
        {
            //FruitSurvivalTracking.Instance.firebase.UpdateResource(resourcesType.ToString(), ((TypeResourcesValue) id).ToString(), source , (int)value, TypeActionResouces.Buy);
        }
        // }

    }

    private void AddResource(TypeResources resourcesType, int id, long value, bool save = true)
    {
        try
        {
            AddResource<UserResource>(resourcesType, id, value);
            if (save)
            {
                GameManager.Instance.SaveDataCreatePlayer();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
        }
    }

    public void SubResource(Item item, string source = "", string sourceId = "", bool save = true)
    {
        SubResource(item.type, item.id, item.value, source, sourceId, save);
    }
    
    public void SubResource(TypeResources type, int idItemOrIdInventory, long value, string source = "", string sourceId = "", bool save = true)
    {
        //FruitSurvivalTracking.Instance.firebase.UpdateResource(type.ToString(), ((TypeResourcesValue) idItemOrIdInventory).ToString(), source , (int)value, TypeActionResouces.Spend);
        
        SubResource(type, idItemOrIdInventory, value, save);
    }

    private void SubResource(TypeResources type, int id, long value, bool save = true)
    {
        try
        {
            GetResource<UserResource>(type).SubValue(id, value);
            if (save)
            {
                GameManager.Instance.SaveDataCreatePlayer();
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
        }
    }

    #region Resources Handle
    public bool ContainStat(TypeResources statType)
    {
        return resourceDict.ContainsKey(statType);
    }
    
    private void AddResource<T>(TypeResources resourceType, ObscuredInt id, ObscuredLong value) where T : UserResource
    {
        var resource = GetResource<T>(resourceType);
        if (resource != null)
        {
            resource.AddValue(id, value);
        }
    }
    
    private T CreateResource<T>(TypeResources resourceType) where T : UserResource
    {
        var stat = System.Activator.CreateInstance<T>();
        resourceDict.Add(resourceType, stat);
        return stat;
    }

    private T CreateOrGetResource<T>(TypeResources resourceType) where T : UserResource
    {
        T stat = GetResource<T>(resourceType);
        if (stat == null)
        {
            stat = CreateResource<T>(resourceType);
        }
        return stat;
    }

    private T GetResource<T>(TypeResources type) where T : UserResource
    {
        return GetResource(type) as T;
    }
    
    private UserResource GetResource(TypeResources resourceType)
    {
        if (!ContainStat(resourceType)) return null;
        resourceDict[resourceType].type = resourceType;
        return resourceDict[resourceType];
    }
    #endregion
    
    public long GetResource(TypeResourcesValue type) { return GetResource<UserResourceItem<ItemResources>>(TypeResources.Resources).GetValue((int)type); }
    public long GetResourceGold() { return GetResource<UserResourceItem<ItemResources>>(TypeResources.Resources).GetValue((int)TypeResourcesValue.Gold); }

    public void InitResources()
    {
        //Resources
        {
            UserResourceItem<ItemResources> resources = CreateOrGetResource<UserResourceItem<ItemResources>>(TypeResources.Resources);
            resources.SetValue(GameManager.Instance.dataCreatePlayer.Inventory.resources);

            // Add resouces on create new data
            if (resources.CollectionsValue.Count == 0)
            {
                // var data = ObjectDataManager.Instance.GeneralConfig;
                // resources.AddOrOverrideValue(data.newbieGem.id, data.newbieGem.value);
                // resources.AddOrOverrideValue(data.newbieGold.id, data.newbieGold.value);
                // resources.AddOrOverrideValue(data.newbieStamina.id, data.newbieStamina.value);
            }

            resources.eventChangeValue = (id, value) =>
            {
                switch ((TypeResourcesValue)id)
                {
                    case TypeResourcesValue.Gold:
                        EventDispatcher.Instance.PostEvent(EventID.ChangeGold);
                        break;
                }
            };
        }
    }
    
}
