using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XFramework;


public delegate void ItemSlotDataSave();

public delegate void ItemSlotDataLoad();

public class ItemSlotDataSaveSceneComponent : SceneComponent
{
    private ItemSlotDataSave _itemSlotDataSaveEvent;
    private ItemSlotDataLoad _itemSlotDataLoadEvent;

    public override void StartComponent()
    {
        AddListenerEvent<int, List<ItemSlot>>("SaveItemSlotSaveDataGroup", SaveItemSlotSaveDataGroup);
        AddReturnListenerEvent<int, ItemSlotSaveDataGroup>("GetItemSlotSaveDataGroup", GetItemSlotSaveDataGroup);
        AddListenerEvent<int, SmallPharmaceuticalApparatusSaveDataGroup>(
            "SaveSmallPharmaceuticalApparatusSaveDataGroup", SaveSmallPharmaceuticalApparatusSaveDataGroup);
        AddReturnListenerEvent<int, SmallPharmaceuticalApparatusSaveDataGroup>(
            "GetSmallPharmaceuticalApparatusSaveDataGroup", GetSmallPharmaceuticalApparatusSaveDataGroup);
        AddListenerEvent<ItemSlotDataSave>("SetItemSlotDataSaveEvent", SetItemSlotDataSaveEvent);
        AddListenerEvent<ItemSlotDataLoad>("SetItemSlotDataLoadEvent", SetItemSlotDataLoadEvent);
        AddListenerEvent("Save", Save);
        AddListenerEvent("Load", Load);
    }

    [AddListenerEvent]
    private void Load()
    {
        _itemSlotDataLoadEvent?.Invoke();
    }

    public override void EndComponent()
    {
    }

    [AddListenerEvent]
    private void Save()
    {
        _itemSlotDataSaveEvent?.Invoke();
    }

    [AddListenerEvent]
    private void SetItemSlotDataSaveEvent(ItemSlotDataSave action)
    {
        _itemSlotDataSaveEvent += action;
    }

    [AddListenerEvent]
    private void SetItemSlotDataLoadEvent(ItemSlotDataLoad action)
    {
        _itemSlotDataLoadEvent += action;
    }

    #region 物品格子

    [AddListenerEvent]
    private void SaveItemSlotSaveDataGroup(int index, List<ItemSlot> itemSlot)
    {
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index +
                                                                             ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ItemSlotSaveDataGroup>(),
                General.assetRootPath + "Save/" + index + ".asset");
        }

        ItemSlotSaveDataGroup itemSlotSaveDataGroup =
            UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index +
                                                                             ".asset");
        itemSlotSaveDataGroup.itemSlotSaveData.Clear();

        foreach (ItemSlot slot in itemSlot)
        {
            if (slot.item != null)
            {
                itemSlotSaveDataGroup.itemSlotSaveData.Add(new ItemSlotSaveData()
                {
                    itemId = slot.item.ItemId, itemSlotIndex = slot.itemIndex,
                    attributeValueList = slot.item.attributeValueList
                });
            }
        }

        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.EditorUtility.SetDirty(itemSlotSaveDataGroup);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    [AddListenerEvent]
    private ItemSlotSaveDataGroup GetItemSlotSaveDataGroup(int index)
    {
        ItemSlotSaveDataGroup itemSlotSaveDataGroup = null;
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index +
                                                                             ".asset") == null)
        {
            return null;
        }
        else
        {
            itemSlotSaveDataGroup =
                UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" +
                    index + ".asset");
        }

        return itemSlotSaveDataGroup;
    }

    #endregion

    #region 小型制药器

    [AddListenerEvent]
    private void SaveSmallPharmaceuticalApparatusSaveDataGroup(int index,
        SmallPharmaceuticalApparatusSaveDataGroup saveSmallPharmaceuticalApparatusSaveDataGroup)
    {
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<SmallPharmaceuticalApparatusSaveDataGroup>(General.assetRootPath +
                "Save/" + index + ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(
                ScriptableObject.CreateInstance<SmallPharmaceuticalApparatusSaveDataGroup>(),
                General.assetRootPath + "Save/" + index + ".asset");
        }

        SmallPharmaceuticalApparatusSaveDataGroup smallPharmaceuticalApparatusSaveDataGroup =
            UnityEditor.AssetDatabase.LoadAssetAtPath<SmallPharmaceuticalApparatusSaveDataGroup>(General.assetRootPath +
                "Save/" + index + ".asset");
        smallPharmaceuticalApparatusSaveDataGroup.currentWater =
            saveSmallPharmaceuticalApparatusSaveDataGroup.currentWater;
        smallPharmaceuticalApparatusSaveDataGroup.incrementWater =
            saveSmallPharmaceuticalApparatusSaveDataGroup.incrementWater;
        smallPharmaceuticalApparatusSaveDataGroup.waterMax = saveSmallPharmaceuticalApparatusSaveDataGroup.waterMax;
        smallPharmaceuticalApparatusSaveDataGroup.currentTemperature =
            saveSmallPharmaceuticalApparatusSaveDataGroup.currentTemperature;
        smallPharmaceuticalApparatusSaveDataGroup.temperatureMax =
            saveSmallPharmaceuticalApparatusSaveDataGroup.temperatureMax;
        smallPharmaceuticalApparatusSaveDataGroup.currentEnergy =
            saveSmallPharmaceuticalApparatusSaveDataGroup.currentEnergy;
        smallPharmaceuticalApparatusSaveDataGroup.temperatureConversionRatio =
            saveSmallPharmaceuticalApparatusSaveDataGroup.temperatureConversionRatio;
        smallPharmaceuticalApparatusSaveDataGroup.everyTimeBurningMultiple =
            saveSmallPharmaceuticalApparatusSaveDataGroup.everyTimeBurningMultiple;
        smallPharmaceuticalApparatusSaveDataGroup.energyBurningTimeTask =
            saveSmallPharmaceuticalApparatusSaveDataGroup.energyBurningTimeTask;
        smallPharmaceuticalApparatusSaveDataGroup.energyBurningEndTimeTask =
            saveSmallPharmaceuticalApparatusSaveDataGroup.energyBurningEndTimeTask;
        smallPharmaceuticalApparatusSaveDataGroup.waterTank = null;
        smallPharmaceuticalApparatusSaveDataGroup.produce = saveSmallPharmaceuticalApparatusSaveDataGroup.produce;
        smallPharmaceuticalApparatusSaveDataGroup.energyTank = saveSmallPharmaceuticalApparatusSaveDataGroup.energyTank;
        smallPharmaceuticalApparatusSaveDataGroup.attributeCompositionDurationDict =
            saveSmallPharmaceuticalApparatusSaveDataGroup.attributeCompositionDurationDict;
        smallPharmaceuticalApparatusSaveDataGroup.combustionInterval =
            saveSmallPharmaceuticalApparatusSaveDataGroup.combustionInterval;
        smallPharmaceuticalApparatusSaveDataGroup.currentCombustionTime =
            saveSmallPharmaceuticalApparatusSaveDataGroup.currentCombustionTime;
        smallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData.Clear();
        smallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData =
            saveSmallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData;
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.EditorUtility.SetDirty(smallPharmaceuticalApparatusSaveDataGroup);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    [AddListenerEvent]
    private SmallPharmaceuticalApparatusSaveDataGroup GetSmallPharmaceuticalApparatusSaveDataGroup(int index)
    {
        SmallPharmaceuticalApparatusSaveDataGroup smallPharmaceuticalApparatusSaveDataGroup = null;
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<SmallPharmaceuticalApparatusSaveDataGroup>(General.assetRootPath +
                "Save/" + index + ".asset") == null)
        {
            return null;
        }
        else
        {
            smallPharmaceuticalApparatusSaveDataGroup =
                UnityEditor.AssetDatabase.LoadAssetAtPath<SmallPharmaceuticalApparatusSaveDataGroup>(
                    General.assetRootPath + "Save/" + index + ".asset");
        }

        return smallPharmaceuticalApparatusSaveDataGroup;
    }

    #endregion
}