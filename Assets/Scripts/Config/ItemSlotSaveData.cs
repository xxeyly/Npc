using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class ItemSlotSaveData
{
    [InfoBox("-1为空的物品格子")] [LabelText("索引")]
    public int itemSlotIndex = -1;

    [LabelText("物品Id")] public int itemId;
    [LabelText("物品属性")] [TableList] public List<AttributeValue> attributeValueList = new List<AttributeValue>();
}