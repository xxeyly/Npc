using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class YearAttributeConfiguration : ScriptableObject
{
    [LabelText("物品")] public Item item;
    [LabelText("成长率")] public float growthRate;
}