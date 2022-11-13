using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class AttributeCompositionSceneComponent : SceneComponent
{
    [LabelText("属性合成组")] public List<AttributeComposition> attributeCompositionGroup = new List<AttributeComposition>();

    public override void StartComponent()
    {
        AddReturnListenerEvent<List<Attribute>, List<AttributeComposition>>("GetQualifiedAttributeComposition", GetQualifiedAttributeComposition);
    }

    public override void InitComponent()
    {
    }

    public override void EndComponent()
    {
    }

    [LabelText("获得合格属性组")]
    private List<AttributeComposition> GetQualifiedAttributeComposition(List<Attribute> attributeGroup)
    {
        List<AttributeComposition> attributeCompositions = new List<AttributeComposition>();
        foreach (AttributeComposition attributeComposition in attributeCompositionGroup)
        {
            //当前属性个数小于数值组个数,不计算
            if (attributeGroup.Count < attributeComposition.attributeGroup.Count)
            {
                continue;
            }

            //属性不合
            if (!AttributeCompositionProportionContain(attributeGroup, attributeComposition.attributeGroup))
            {
                continue;
            }

            attributeCompositions.Add(attributeComposition);
        }

        attributeCompositions = AttributeCompositionSort(attributeCompositions);

        return attributeCompositions;
    }

    //排序,大的放前面
    private List<AttributeComposition> AttributeCompositionSort(List<AttributeComposition> attributeCompositions)
    {
        for (int i = 0; i < attributeCompositions.Count; i++)
        {
            for (int j = i; j < attributeCompositions.Count; j++)
            {
                if (attributeCompositions[i].priority < attributeCompositions[j].priority)
                {
                    (attributeCompositions[i], attributeCompositions[j]) = (attributeCompositions[j], attributeCompositions[i]);
                }
            }
        }

        return attributeCompositions;
    }

    [LabelText("比较两个属性是否包含")]
    private bool AttributeCompositionProportionContain(List<Attribute> compareAttributeGroup, List<AttributeCompositionProportion> primaryAttributeGroup)
    {
        List<int> compareAttribute = new List<int>();

        foreach (Attribute attribute in compareAttributeGroup)
        {
            compareAttribute.Add(attribute.attributeId);
        }

        List<int> primaryAttribute = new List<int>();

        foreach (AttributeCompositionProportion attributeCompositionProportion in primaryAttributeGroup)
        {
            primaryAttribute.Add(attributeCompositionProportion.attribute.attributeId);
        }

        for (int i = 0; i < primaryAttribute.Count; i++)
        {
            if (!compareAttribute.Contains(primaryAttribute[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeGroup">属性组</param>
    /// <param name="priority"></param>
    /// <param name="requiredTime"></param>
    /// <param name="requiredTemperature"></param>
    /// <param name="finalAttribute"></param>
    [BoxGroup]
    [Button("创建属性配方")]
    public void CreateItem(List<AttributeCompositionProportion> attributeGroup, int priority, int requiredTime, int requiredTemperature, Attribute finalAttribute)
    {
        AttributeComposition attributeComposition = ScriptableObject.CreateInstance<AttributeComposition>();
        attributeComposition.attributeGroup = attributeGroup;
        attributeComposition.priority = priority;
        attributeComposition.requiredTime = requiredTime;
        attributeComposition.requiredTemperature = requiredTemperature;
        attributeComposition.finalAttribute = finalAttribute;
        //创建新的物品
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<Item>(General.assetRootPath + "AttributeComposition/" + finalAttribute.AttributeName + "配方" + ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(attributeComposition, General.assetRootPath + "AttributeComposition/" + finalAttribute.AttributeName + "配方" + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}