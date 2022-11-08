using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemSlotSaveDataGroup : ScriptableObject
{
    public List<ItemSlotSaveData> itemSlotSaveData = new List<ItemSlotSaveData>();
}

[Serializable]
public class ItemSlotSaveData
{
    [LabelText("索引")] public int itemSlotIndex;
    [LabelText("物品Id")] public int itemId;
    [LabelText("物品属性")] [TableList] public List<AttributeValue> attributeValueList = new List<AttributeValue>();
}