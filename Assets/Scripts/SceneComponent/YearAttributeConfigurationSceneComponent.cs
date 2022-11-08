using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

public class YearAttributeConfigurationSceneComponent : SceneComponent
{
    [LabelText("物品成长配置")] public List<YearAttributeConfiguration> yearAttributeConfigurationConfig;

    public override void StartComponent()
    {
    }

    public override void InitComponent()
    {
    }

    public override void EndComponent()
    {
    }

    [Button("创建成长率")]
    public void CreateYearAttributeConfiguration(Item item, float growthRate)
    {
        YearAttributeConfiguration attributeComposition = ScriptableObject.CreateInstance<YearAttributeConfiguration>();
        attributeComposition.item = item;
        attributeComposition.growthRate = growthRate;
        //创建新的物品
        if (UnityEditor.AssetDatabase.LoadAssetAtPath<YearAttributeConfiguration>(General.assetRootPath + "YearAttributeConfiguration/" + item.ItemName + "成长率" + ".asset") == null)
        {
            UnityEditor.AssetDatabase.CreateAsset(attributeComposition, General.assetRootPath + "YearAttributeConfiguration/" + item.ItemName + "成长率" + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}