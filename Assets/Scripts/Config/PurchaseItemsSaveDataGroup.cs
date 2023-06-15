using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PurchaseItemsSaveDataGroup : ItemSlotSaveDataGroup
{
    [LabelText("收购物品数据")] [SerializeField] public Dictionary<ItemDemandItem, ItemDemandItemData> itemDemandItemDataDic = new Dictionary<ItemDemandItem,ItemDemandItemData>();

}