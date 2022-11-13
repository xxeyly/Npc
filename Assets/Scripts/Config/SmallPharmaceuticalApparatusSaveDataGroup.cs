using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SmallPharmaceuticalApparatusSaveDataGroup : ItemSlotSaveDataGroup
{
    [BoxGroup("水量")] [LabelText("当前水量")] [SerializeField]
    public int currentWater;

    [BoxGroup("水量")] [LabelText("水量增加数")] [SerializeField]
    public int incrementWater;

    [BoxGroup("水量")] [LabelText("水量上限")] [SerializeField]
    public int waterMax;


    [BoxGroup("温度")] [LabelText("当前温度")] [SerializeField]
    public int currentTemperature;

    [BoxGroup("温度")] [LabelText("温度上限")] [SerializeField]
    public int temperatureMax;

    [BoxGroup("温度")] [LabelText("当前能量")] [SerializeField]
    public float currentEnergy;

    [BoxGroup("温度")] [LabelText("能量缓存")] public float energyCache;

    [BoxGroup("温度")] [LabelText("温度换算比 1秒1温度等于多少能量")]
    public int temperatureConversionRatio;


    [BoxGroup("温度")] [LabelText("每次燃烧需要的倍数")] [SerializeField]
    public int everyTimeBurningMultiple;

    [BoxGroup("温度")] [LabelText("能量燃烧任务")] public int energyBurningTimeTask;

    [BoxGroup("温度")] [LabelText("能量燃烧结束任务")]
    public int energyBurningEndTimeTask;

    public ItemSlotSaveData waterTank;
    public ItemSlotSaveData produce;
    public ItemSlotSaveData energyTank;
    [SerializeField] [LabelText("配方持续时间")] public Dictionary<AttributeComposition, int> attributeCompositionDurationDict = new Dictionary<AttributeComposition, int>();

    [SerializeField] [LabelText("燃烧间隔")] public int combustionInterval = 1;
    [SerializeField] [LabelText("当前燃烧时间")] public float currentCombustionTime = 0;
}