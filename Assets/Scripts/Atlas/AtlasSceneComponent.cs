using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

#if UNITY_EDITOR
public class AtlasSceneComponent : SceneComponent
{
    [LabelText("物品图鉴")] [SerializeField] [Searchable] [TableList(AlwaysExpanded = true)] [InlineEditor()]
    private ItemAtlas itemAtlas;

    [LabelText("解锁图鉴")] [SerializeField] private List<Item> unlockingItem = new List<Item>();

    [LabelText("图鉴路径")] [SerializeField] [FolderPath(AbsolutePath = true)]
    private string itemAtlasPath;

    [Button("创建图鉴")]
    public void CreateItemAtlas()
    {
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<ItemAtlas>(General.assetRootPath + "ItemAtlas.asset") != null)
        {
            itemAtlas = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemAtlas>(General.assetRootPath + "ItemAtlas.asset");
            return;
        }

        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ItemAtlas>(), General.assetRootPath + "ItemAtlas.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        itemAtlas = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemAtlas>(General.assetRootPath + "ItemAtlas.asset");
    }

    [Button("创建属性")]
    public void CreateAttribute()
    {
        List<Attribute> attributes = DataFrameComponent.GetInheritAllSubclass<Attribute>();
        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case CapacityAttribute capacityAttribute:
                    CreateAttribute<CapacityAttribute>("容量");
                    break;
                case EarthEnergyAttribute earthEnergyAttribute:
                    CreateAttribute<EarthEnergyAttribute>("土能量");
                    break;
                case EnergyAttribute energyAttribute:
                    CreateAttribute<EnergyAttribute>("能量");
                    break;
                case FireAttribute fireAttribute:
                    CreateAttribute<FireAttribute>("火");
                    break;
                case FireEnergyAttribute fireEnergyAttribute:
                    CreateAttribute<FireEnergyAttribute>("火能量");
                    break;
                case HealthyAttribute healthyAttribute:
                    CreateAttribute<HealthyAttribute>("健康");
                    break;
                case MetalEnergyAttribute metalEnergyAttribute:
                    CreateAttribute<MetalEnergyAttribute>("金能量");
                    break;
                case ParticularYearAttribute particularYearAttribute:
                    CreateAttribute<ParticularYearAttribute>("年份");
                    break;
                case WaterAttribute waterAttribute:
                    CreateAttribute<WaterAttribute>("水");
                    break;
                case WaterEnergyAttribute waterEnergyAttribute:
                    CreateAttribute<WaterEnergyAttribute>("水能量");
                    break;
                case WoodEnergyAttribute woodEnergyAttribute:
                    CreateAttribute<WoodEnergyAttribute>("木能量");
                    break;
            }
        }
    }

    private void CreateAttribute<T>(string attributeName) where T : Attribute
    {
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<T>(General.assetRootPath + "Attribute/" + attributeName + ".asset") != null)
        {
            return;
        }

        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<T>(), General.assetRootPath + "Attribute/" + attributeName + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }


    private Item CreateItemByItemId(int itemId)
    {
        Item item = null;
        foreach (Item itemAtlasItem in itemAtlas.Items)
        {
            if (itemAtlasItem.ItemId == itemId)
            {
                item = itemAtlasItem.GetNewItem();
            }
        }

        return item;
    }

    [BoxGroup]
    [Button("创建物品")]
    public void CreateItem(string itemScriptableObjectName, string itemName, string describe, Sprite itemIcon, List<AttributeValue> itemAttribute)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.ItemId = GetItemNotRepeatId();
        item.ItemName = itemName;
        item.Describe = describe;
        item.ItemIcon = itemIcon;
        item.attributeValueList = itemAttribute;
        //创建新的物品
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<Item>(General.assetRootPath + "Item/" + itemScriptableObjectName + ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(item, General.assetRootPath + "Item/" + itemScriptableObjectName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        itemAtlas.Items.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Item>(General.assetRootPath + "Item/" + itemScriptableObjectName + ".asset"));
    }

    //获得物品物品列表不重复Id
    private int GetItemNotRepeatId()
    {
        for (int i = 0; i < itemAtlas.Items.Count; i++)
        {
            if (i != itemAtlas.Items[i].ItemId)
            {
                return i;
            }
        }

        return itemAtlas.Items.Count;
    }

    public override void StartComponent()
    {
        AddReturnListenerEvent<Item, bool>("GetItemUnlocking", GetItemUnlocking);
        AddReturnListenerEvent<int, Item>("CreateItemByItemId", CreateItemByItemId);
        AddReturnListenerEvent<ItemAtlas>("GetItemAtlas", GetItemAtlas);
        AddReturnListenerEvent<List<Item>>("GetUnlockingItem", GetUnlockingItem);
    }


    public override void EndComponent()
    {
    }

    private bool GetItemUnlocking(Item item)
    {
        return unlockingItem.Contains(item);
    }

    private ItemAtlas GetItemAtlas()
    {
        return itemAtlas;
    }

    private List<Item> GetUnlockingItem()
    {
        return unlockingItem;
    }
}
#endif