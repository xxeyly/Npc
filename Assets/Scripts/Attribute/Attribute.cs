using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Attribute : ScriptableObject
{
    [LabelText("属性iD")] public int attributeId;
    [LabelText("属性名称")] public string AttributeName;
}