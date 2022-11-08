﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[InfoBox("属性合成")]
[Serializable]
public class AttributeComposition : ScriptableObject
{
    [LabelText("ID")] public int id;
    [LabelText("属性组")] public List<AttributeCompositionProportion> attributeGroup = new List<AttributeCompositionProportion>();
    [LabelText("优先级")] public int priority;
    [LabelText("所需时间")] public int requiredTime;
    [LabelText("所需温度")] public int requiredTemperature;
    [LabelText("最终属性")] public Attribute finalAttribute;
}

[Serializable]
[InfoBox("属性合成比例")]
public class AttributeCompositionProportion
{
    [LabelText("属性")] public Attribute attribute;
    [LabelText("比例值")] public int value;
}