using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemAtlas : ScriptableObject
{
    [LabelText("物品列表")] public List<Item> Items = new List<Item>();
}