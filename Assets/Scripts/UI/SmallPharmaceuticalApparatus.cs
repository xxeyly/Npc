//引入开始

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//引入结束
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

public class SmallPharmaceuticalApparatus : BaseWindow
{
    //变量声明开始
    private Button _windowMoveEvent;
    private Button _close;
    private List<ItemSlot> _itemSlotContent;
    private Scrollbar _waterProgress;
    private TextMeshProUGUI _currentWaterValue;
    private TextMeshProUGUI _waterValueMax;
    private Button _increaseWater;
    private Scrollbar _energyProgress;
    private TextMeshProUGUI _currentEnergyValue;
    private Button _increaseTemperature;
    private Button _reduceTemperature;

    private TextMeshProUGUI _temperatureValue;

    //变量声明结束
    [SerializeField] [LabelText("移动布局")] private bool moveWindow;
    [BoxGroup("水量")] [LabelText("移动偏移")] private Vector3 _moveOffset;

    [BoxGroup("水量")] [LabelText("当前水量")] [SerializeField]
    private int currentWater;

    [BoxGroup("水量")] [LabelText("水量增加数")] [SerializeField]
    private int incrementWater;

    [BoxGroup("水量")] [LabelText("水量上限")] [SerializeField]
    private int waterMax;

    private ItemSlot _waterTank;

    [BoxGroup("温度")] [LabelText("当前温度")] [SerializeField]
    private int currentTemperature;

    [BoxGroup("温度")] [LabelText("温度上限")] [SerializeField]
    private int temperatureMax;

    [BoxGroup("温度")] [LabelText("当前能量")] [SerializeField]
    private float currentEnergy;

    [BoxGroup("温度")] [LabelText("能量缓存")] public float energyCache;

    [BoxGroup("温度")] [LabelText("温度换算比 1秒1温度等于多少能量")]
    public int temperatureConversionRatio;


    [BoxGroup("温度")] [LabelText("每次燃烧需要的倍数")] [SerializeField]
    private int everyTimeBurningMultiple;

    [BoxGroup("温度")] [LabelText("能量燃烧任务")] private int _energyBurningTimeTask;

    [BoxGroup("温度")] [LabelText("能量燃烧结束任务")]
    private int _energyBurningEndTimeTask;

    private ItemSlot _energyTank;
    [SerializeField] [LabelText("配方持续时间")] private Dictionary<AttributeComposition, int> attributeCompositionDurationDict = new Dictionary<AttributeComposition, int>();

    [SerializeField] [LabelText("燃烧间隔")] private int combustionInterval = 1;
    [SerializeField] [LabelText("当前燃烧时间")] private float currentCombustionTime = 0;

    [LabelText("能量重置")]
    private void EnergyReset()
    {
        DeleteTimeTask(_energyBurningTimeTask);
        DeleteTimeTask(_energyBurningEndTimeTask);
        //剩余的能力缓存大于一波燃烧能量值
        if (energyCache >= currentTemperature * temperatureConversionRatio * everyTimeBurningMultiple)
        {
            //减去一波燃烧值
            energyCache -= currentTemperature * temperatureConversionRatio * everyTimeBurningMultiple;
            _energyBurningTimeTask = AddTimeTask(EnergyBurning, "能量燃烧", 1, everyTimeBurningMultiple);
            _energyBurningEndTimeTask = AddTimeTask(EnergyBurningEnd, "能量燃烧结束", everyTimeBurningMultiple);
        }
        //小于一波能量值
        else
        {
            //剩余的能量值可以补给能量缓存
            if (currentEnergy > currentTemperature * temperatureConversionRatio * everyTimeBurningMultiple - energyCache)
            {
                currentEnergy -= currentTemperature * temperatureConversionRatio * everyTimeBurningMultiple - energyCache;
                energyCache += currentTemperature * temperatureConversionRatio * everyTimeBurningMultiple - energyCache;
                _currentEnergyValue.text = currentEnergy.ToString();
                _energyBurningTimeTask = AddTimeTask(EnergyBurning, "能量燃烧", 1, everyTimeBurningMultiple);
                _energyBurningEndTimeTask = AddTimeTask(EnergyBurningEnd, "能量燃烧结束", everyTimeBurningMultiple);
            }
            //剩余能量值已经不够补给
            else if (currentEnergy > 0)
            {
                energyCache += currentEnergy;
                currentEnergy = 0;
                int surplusBurning = (int)energyCache / (currentTemperature * temperatureConversionRatio);
                Debug.Log("只能燃烧:" + surplusBurning);
                if (surplusBurning == 0)
                {
                    Debug.Log("能量不够燃烧一次的");
                }
                else
                {
                    _currentEnergyValue.text = currentEnergy.ToString();
                    _energyBurningTimeTask = AddTimeTask(EnergyBurning, "能量燃烧", 1, surplusBurning);
                    _energyBurningEndTimeTask = AddTimeTask(EnergyBurningEnd, "能量燃烧结束", surplusBurning);
                }
            }
            else if (currentEnergy == 0)
            {
                Debug.Log("已经没能量了");
            }
        }
    }

    private void EnergyBurning()
    {
        energyCache -= currentTemperature * temperatureConversionRatio;
    }

    private void EnergyBurningEnd()
    {
        Debug.Log("一波能量燃烧完毕");
        //还有缓存能量
        if (energyCache > 0)
        {
            EnergyReset();
        }
        else if (currentEnergy > 0)
        {
            EnergyReset();
        }
        else
        {
            Debug.Log("没能量了");
        }
    }

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _windowMoveEvent, "Title/WindowMoveEvent");
        BindUi(ref _close, "Title/Close");
        BindUi(ref _itemSlotContent, "ItemSlotContent");
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].ViewStartInit();
            _itemSlotContent[i].InitData(i);
        }

        BindUi(ref _waterProgress, "WaterPanel/WaterProgress");
        BindUi(ref _currentWaterValue, "WaterPanel/CurrentWaterValue");
        BindUi(ref _waterValueMax, "WaterPanel/WaterValueMax");
        BindUi(ref _increaseWater, "WaterPanel/IncreaseWater");
        BindUi(ref _energyProgress, "EnergyPanel/EnergyProgress");
        BindUi(ref _currentEnergyValue, "EnergyPanel/CurrentEnergyValue");
        BindUi(ref _increaseTemperature, "TemperaturePanel/IncreaseTemperature");
        BindUi(ref _reduceTemperature, "TemperaturePanel/ReduceTemperature");
        BindUi(ref _temperatureValue, "TemperaturePanel/TemperatureValue");
        //变量查找结束
        BindUi(ref _waterTank, "WaterPanel/WaterTank");
        BindUi(ref _energyTank, "EnergyPanel/EnergyTank");
        _waterTank.ViewStartInit();
        _energyTank.ViewStartInit();
        _waterTank.PlaceItemEvent += WaterTankPlaceItemEvent;
        _waterTank.RemoveItemEvent += WaterTankRemoveItemEvent;

        _energyTank.PlaceItemEvent += EnergyTankPlaceItemEvent;
        _energyTank.RemoveItemEvent += EnergyTankRemoveItemEvent;
    }


    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);
        BindListener(_increaseWater, EventTriggerType.PointerClick, OnIncreaseWaterClick);
        BindListener(_increaseTemperature, EventTriggerType.PointerClick, OnIncreaseTemperatureClick);
        BindListener(_reduceTemperature, EventTriggerType.PointerClick, OnReduceTemperatureClick);
        //变量绑定结束
    }

    //变量方法开始
    private void OnWindowMoveEventDown(BaseEventData targetObj)
    {
        ListenerComponent.Instance.itemAttributeShow.HideItemAttribute();
        ListenerComponent.Instance.itemAttributeShow.SetWindowDrag(true);

        _moveOffset = window.transform.position - Input.mousePosition;
        moveWindow = true;
    }

    private void OnWindowMoveEventUp(BaseEventData targetObj)
    {
        ListenerComponent.Instance.itemAttributeShow.SetWindowDrag(false);

        moveWindow = false;
    }

    private void OnCloseClick(BaseEventData targetObj)
    {
        HideThisView();
    }

    private void OnIncreaseWaterClick(BaseEventData targetObj)
    {
        int remainingWater = waterMax - currentWater;
        AttributeValue waterAttribute = null;
        foreach (AttributeValue attributeValue in _waterTank.item.attributeValueList)
        {
            if (attributeValue.attribute.GetType() == typeof(WaterAttribute))
            {
                waterAttribute = attributeValue;
            }
        }


        //当前没有找到水的属性,返回
        if (waterAttribute == null)
        {
            return;
        }
        else
        {
            Debug.Log("没有水属性");
        }

        //大于最大一次增加数
        if (remainingWater >= incrementWater)
        {
            //水瓶中大于最大数量
            if (waterAttribute.value >= incrementWater)
            {
                currentWater += incrementWater;
                waterAttribute.value -= incrementWater;
            }
            else
            {
                currentWater += waterAttribute.value;
                waterAttribute.value = 0;
            }
        }
        else
        {
            //水瓶中大于最大数量
            if (waterAttribute.value >= remainingWater)
            {
                currentWater += remainingWater;
                waterAttribute.value -= remainingWater;
            }
            else
            {
                currentWater += waterAttribute.value;
                waterAttribute.value = 0;
            }
        }

        _waterTank.UpdateItemUI();
        if (currentWater < 100)
        {
            _currentWaterValue.text = "0" + currentWater.ToString();
        }
        else
        {
            _currentWaterValue.text = currentWater.ToString();
        }
    }

    private void OnIncreaseTemperatureClick(BaseEventData targetObj)
    {
        if (currentEnergy <= 0)
        {
            return;
        }

        if (currentTemperature < temperatureMax)
        {
            currentTemperature += 1;
            _temperatureValue.text = currentTemperature.ToString();
            EnergyReset();
        }
    }

    private void OnReduceTemperatureClick(BaseEventData targetObj)
    {
        if (currentEnergy <= 0)
        {
            return;
        }

        if (currentTemperature > 1)
        {
            currentTemperature -= 1;
            _temperatureValue.text = currentTemperature.ToString();
            EnergyReset();
        }
    }
    //变量方法结束

    //自定义属性开始

    //自定义属性结束

    private void WaterTankPlaceItemEvent(Item item)
    {
        Debug.Log("放入");
    }

    private void WaterTankRemoveItemEvent()
    {
        Debug.Log("拿出");
    }

    private void EnergyTankRemoveItemEvent()
    {
        currentEnergy = 0;
        _currentEnergyValue.text = currentEnergy.ToString();
    }

    private void EnergyTankPlaceItemEvent(Item item)
    {
        currentEnergy = 0;
        foreach (AttributeValue attributeValue in item.attributeValueList)
        {
            currentEnergy += attributeValue.value;
        }

        _currentEnergyValue.text = currentEnergy.ToString();
    }

    [LabelText("获得容器内所有属性")]
    private List<Attribute> GetAllContainerAttribute()
    {
        List<Attribute> attributes = new List<Attribute>();

        foreach (ItemSlot itemSlot in _itemSlotContent)
        {
            if (itemSlot.item == null)
            {
                continue;
            }

            foreach (AttributeValue attributeValue in itemSlot.item.attributeValueList)
            {
                if (!attributes.Contains(attributeValue.attribute))
                {
                    attributes.Add(attributeValue.attribute);
                }
            }
        }

        return attributes;
    }


    protected override void Update()
    {
        base.Update();
        if (moveWindow)
        {
            window.transform.position = Input.mousePosition + _moveOffset;
        }

        //当前有能量
        if (currentEnergy > 0)
        {
        }

        currentCombustionTime += Time.deltaTime;
        if (currentCombustionTime >= combustionInterval)
        {
            currentCombustionTime = 0;
            AttributeComposition attributeComposition = ListenerComponent.Instance.attributeCompositionSceneComponent.GetQualifiedAttributeComposition(GetAllContainerAttribute(), currentTemperature);
            if (!attributeCompositionDurationDict.ContainsKey(attributeComposition))
            {
                attributeCompositionDurationDict.Add(attributeComposition, 1);
            }
            else
            {
                attributeCompositionDurationDict[attributeComposition] += 1;
                //配方时间到了
                if (attributeCompositionDurationDict[attributeComposition] >= attributeComposition.requiredTime)
                {
                    
                }
            }
        }
    }
}