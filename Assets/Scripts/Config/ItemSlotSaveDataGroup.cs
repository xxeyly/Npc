using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ItemSlotSaveDataGroup : ScriptableObject
{
    public List<ItemSlotSaveData> itemSlotSaveData = new List<ItemSlotSaveData>();
}

