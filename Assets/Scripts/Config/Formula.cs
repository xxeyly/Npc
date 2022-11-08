using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[InfoBox("配方")]
public class Formula : ScriptableObject
{
}

[Serializable]
[InfoBox("配方步骤")]
public class FormulaStep
{
    [LabelText("主药")] public Item mainDrug;
    [LabelText("辅药")] public List<Item> adjuvants;
    [LabelText("药引")] public Item medicineInducer;
    [LabelText("温度")] public int temperature;
    [LabelText("温度持续时间")] public int temperatureDuration;
}