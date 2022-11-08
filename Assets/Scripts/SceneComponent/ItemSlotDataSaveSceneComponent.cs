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
        AddListenerEvent<ItemSlotDataSave>("SetItemSlotDataSaveEvent", SetItemSlotDataSaveEvent);
        AddListenerEvent<ItemSlotDataLoad>("SetItemSlotDataLoadEvent", SetItemSlotDataLoadEvent);
        AddListenerEvent("Save", Save);
        AddListenerEvent("Load", Load);
    }

    private void Load()
    {
        _itemSlotDataLoadEvent?.Invoke();
    }

    public override void InitComponent()
    {
    }

    public override void EndComponent()
    {
    }

    private void Save()
    {
        _itemSlotDataSaveEvent?.Invoke();
    }

    private void SetItemSlotDataSaveEvent(ItemSlotDataSave action)
    {
        _itemSlotDataSaveEvent += action;
    }

    private void SetItemSlotDataLoadEvent(ItemSlotDataLoad action)
    {
        _itemSlotDataLoadEvent += action;
    }


    private void SaveItemSlotSaveDataGroup(int index, List<ItemSlot> itemSlot)
    {
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index + ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ItemSlotSaveDataGroup>(), General.assetRootPath + "Save/" + index + ".asset");
        }

        ItemSlotSaveDataGroup itemSlotSaveDataGroup = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index + ".asset");
        itemSlotSaveDataGroup.itemSlotSaveData.Clear();

        foreach (ItemSlot slot in itemSlot)
        {
            if (slot.item != null)
            {
                itemSlotSaveDataGroup.itemSlotSaveData.Add(new ItemSlotSaveData()
                {
                    itemId = slot.item.ItemId, itemSlotIndex = slot.itemIndex, attributeValueList = slot.item.attributeValueList
                });
            }
        }

        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.EditorUtility.SetDirty(itemSlotSaveDataGroup);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    private ItemSlotSaveDataGroup GetItemSlotSaveDataGroup(int index)
    {
        ItemSlotSaveDataGroup itemSlotSaveDataGroup = null;
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index + ".asset") == null)
        {
            return null;
        }
        else
        {
            itemSlotSaveDataGroup = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemSlotSaveDataGroup>(General.assetRootPath + "Save/" + index + ".asset");
        }

        return itemSlotSaveDataGroup;
    }
}