using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
[InlineEditor()]
public class Item : ScriptableObject
{
    [LabelText("物品ID")] public int ItemId;
    [LabelText("名称")] public string ItemName;
    [LabelText("描述")] public string Describe;

    [LabelText("物品图片")] [PreviewField(55)] [LabelWidth(55)]
    public Sprite ItemIcon;

    [LabelText("物品属性")] [TableList] public List<AttributeValue> attributeValueList = new List<AttributeValue>();

    public bool ContainsAttribute(BaseAttribute baseAttribute)
    {
        foreach (AttributeValue attributeValue in attributeValueList)
        {
            if (attributeValue.baseAttribute.GetAttributeType() == baseAttribute.GetAttributeType())
            {
                return true;
            }
        }

        return false;
    }

    public void AddAttributeValue(AttributeValue attributeValue)
    {
        //相同属性
        bool identical = false;
        foreach (AttributeValue value in attributeValueList)
        {
            if (value.baseAttribute.GetAttributeType() == attributeValue.baseAttribute.GetAttributeType())
            {
                value.value += attributeValue.value;
                identical = true;
            }
        }

        if (!identical)
        {
            attributeValueList.Add(attributeValue);
        }
    }

    public Item GetNewItem()
    {
        Item item = CreateInstance<Item>();
        item.ItemId = ItemId;
        item.ItemName = ItemName;
        item.ItemIcon = ItemIcon;
        item.Describe = Describe;
        item.attributeValueList = attributeValueList;


        return item;
    }
}

[Serializable]
public class AttributeValue
{
    [FormerlySerializedAs("attribute")] public BaseAttribute baseAttribute;
    public int value;
}