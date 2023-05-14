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

    private ItemSlot _waterTank;
    private ItemSlot _energyTank;
    private ItemSlot _produce;
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
        // Debug.Log("一波能量燃烧完毕");
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
        BindUi(ref _produce, "ProducePanel/Produce");
        _waterTank.ViewStartInit();
        _energyTank.ViewStartInit();
        _produce.ViewStartInit();
        //获得临时属性格子
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
        AddListenerEvent("InitStorageItem", InitStorageItem);
    }

    //变量方法开始
    private void OnWindowMoveEventDown(BaseEventData targetObj)
    {
        ListenerFrameComponent.Instance.itemAttributeShow.HideItemAttribute();
        ListenerFrameComponent.Instance.itemAttributeShow.SetWindowDrag(true);

        _moveOffset = window.transform.position - Input.mousePosition;
        moveWindow = true;
    }

    private void OnWindowMoveEventUp(BaseEventData targetObj)
    {
        ListenerFrameComponent.Instance.itemAttributeShow.SetWindowDrag(false);

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

    [LabelText("获得容器内所有属性,0除外")]
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
                //没有包含当前属性,并且属性有值
                if (!attributes.Contains(attributeValue.attribute) && attributeValue.value > 0)
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

        //没能量,温度
        if (currentEnergy == 0 && energyCache == 0)
        {
            currentTemperature = 0;
            return;
        }

        currentCombustionTime += Time.deltaTime;
        if (currentCombustionTime >= combustionInterval)
        {
            currentCombustionTime = 0;
            //获得所有配方
            List<AttributeComposition> attributeCompositions = ListenerFrameComponent.Instance.attributeCompositionSceneComponent.GetQualifiedAttributeComposition(GetAllContainerAttribute());
            foreach (AttributeComposition attributeComposition in attributeCompositions)
            {
                //包含配方全部属性,材料充足
                if (GetSufficientMaterials(attributeComposition))
                {
                    //没有属性添加属性
                    if (!attributeCompositionDurationDict.ContainsKey(attributeComposition))
                    {
                        attributeCompositionDurationDict.Add(attributeComposition, 0);
                    }

                    //符合合适温度
                    if (currentTemperature == attributeComposition.requiredTemperature)
                    {
                        attributeCompositionDurationDict[attributeComposition] += 1;
                    }
                    else
                    {
                        //温度不合适,减少
                        if (attributeCompositionDurationDict[attributeComposition] > 0)
                        {
                            attributeCompositionDurationDict[attributeComposition] -= 1;
                        }
                    }
                }
                else
                {
                    if (attributeCompositionDurationDict.ContainsKey(attributeComposition))
                    {
                        attributeCompositionDurationDict.Remove(attributeComposition);
                    }
                }

                //配方时间到了
                if (attributeCompositionDurationDict[attributeComposition] >= attributeComposition.requiredTime)
                {
                    RemoveMaterialsGenerateNewAttribute(attributeComposition);
                    //移除当前配方
                    if (attributeCompositionDurationDict.ContainsKey(attributeComposition))
                    {
                        attributeCompositionDurationDict.Remove(attributeComposition);
                    }
                }
            }
        }
    }

    //当前配方所需的材料是否充足
    private bool GetSufficientMaterials(AttributeComposition attributeComposition)
    {
        bool sufficientMaterials = true;
        foreach (AttributeCompositionProportion attributeCompositionProportion in attributeComposition.attributeGroup)
        {
            //需要的值
            int needValue = attributeCompositionProportion.value;
            //当前的值
            int currentValue = 0;
            //获得所有物品的属性值
            foreach (ItemSlot itemSlot in _itemSlotContent)
            {
                if (itemSlot.item == null)
                {
                    continue;
                }

                foreach (AttributeValue attributeValue in itemSlot.item.attributeValueList)
                {
                    if (attributeValue.attribute.attributeId == attributeCompositionProportion.attribute.attributeId)
                    {
                        currentValue += attributeValue.value;
                    }
                }
            }

            //材料不充足
            if (currentValue < needValue)
            {
                sufficientMaterials = false;
            }
        }

        return sufficientMaterials;
    }

    //移除材料并生成新的物体
    private void RemoveMaterialsGenerateNewAttribute(AttributeComposition attributeComposition)
    {
        foreach (AttributeCompositionProportion attributeCompositionProportion in attributeComposition.attributeGroup)
        {
            //需要的值
            int needValue = attributeCompositionProportion.value;
            //材料充足,移除物品属性,生成新的物品
            foreach (ItemSlot itemSlot in _itemSlotContent)
            {
                //当前物品是否包含当前属性
                if (itemSlot.item == null)
                {
                    continue;
                }

                foreach (AttributeValue attributeValue in itemSlot.item.attributeValueList)
                {
                    //找到属性一样的
                    if (attributeCompositionProportion.attribute.attributeId == attributeValue.attribute.attributeId)
                    {
                        if (needValue <= attributeValue.value)
                        {
                            attributeValue.value -= needValue;
                        }
                        else
                        {
                            needValue -= attributeValue.value;
                            attributeValue.value = 0;
                        }
                    }
                }
            }
        }

        //如果没有放置容器,直接丢失
        if (_produce.item == null)
        {
            return;
        }

        //当前量
        int currentValue = 0;
        //算出瓶子还有多少容量
        int remainingCapacity = 0;
        //总容量
        int totalCapacity = 0;
        foreach (AttributeValue attributeValue in _produce.item.attributeValueList)
        {
            //容量属性不能计算
            if (attributeValue.attribute.GetAttributeType() != typeof(CapacityAttribute))
            {
                currentValue += attributeValue.value;
            }
            else
            {
                totalCapacity += attributeValue.value;
            }
        }

        //剩余容量
        remainingCapacity = totalCapacity - currentValue;
        //产出量
        int outputValue = attributeComposition.OutputValue();

        if (outputValue <= remainingCapacity)
        {
            _produce.item.AddAttributeValue(new AttributeValue()
            {
                attribute = attributeComposition.finalAttribute,
                value = outputValue
            });
        }
        else
        {
            _produce.item.AddAttributeValue(new AttributeValue()
            {
                attribute = attributeComposition.finalAttribute,
                value = remainingCapacity
            });
        }
    }

    private void InitStorageItem()
    {
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataSaveEvent(Save);
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataLoadEvent(Load);
    }

    private void Save()
    {
        SmallPharmaceuticalApparatusSaveDataGroup smallPharmaceuticalApparatusSaveDataGroup = SaveSmallPharmaceuticalApparatusSaveDataGroup(currentWater, incrementWater, waterMax, currentTemperature, temperatureMax, currentEnergy, temperatureConversionRatio, everyTimeBurningMultiple,
            _energyBurningTimeTask, _energyBurningEndTimeTask, _waterTank, _produce, _energyTank, attributeCompositionDurationDict, combustionInterval, currentCombustionTime, _itemSlotContent);
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SaveSmallPharmaceuticalApparatusSaveDataGroup(1, smallPharmaceuticalApparatusSaveDataGroup);
    }

    private SmallPharmaceuticalApparatusSaveDataGroup SaveSmallPharmaceuticalApparatusSaveDataGroup(int currentWater, int incrementWater, int waterMax, int currentTemperature, int temperatureMax, float currentEnergy, int temperatureConversionRatio, int everyTimeBurningMultiple,
        int energyBurningTimeTask, int energyBurningEndTimeTask, ItemSlot waterTank, ItemSlot produce, ItemSlot energyTank, Dictionary<AttributeComposition, int> attributeCompositionDurationDict, int combustionInterval, float currentCombustionTime, List<ItemSlot> itemSlot)
    {
        SmallPharmaceuticalApparatusSaveDataGroup smallPharmaceuticalApparatusSaveDataGroup = ScriptableObject.CreateInstance<SmallPharmaceuticalApparatusSaveDataGroup>();
        smallPharmaceuticalApparatusSaveDataGroup.currentWater = currentWater;
        smallPharmaceuticalApparatusSaveDataGroup.incrementWater = incrementWater;
        smallPharmaceuticalApparatusSaveDataGroup.waterMax = waterMax;
        smallPharmaceuticalApparatusSaveDataGroup.currentTemperature = currentTemperature;
        smallPharmaceuticalApparatusSaveDataGroup.temperatureMax = temperatureMax;
        smallPharmaceuticalApparatusSaveDataGroup.currentEnergy = currentEnergy;
        smallPharmaceuticalApparatusSaveDataGroup.temperatureConversionRatio = temperatureConversionRatio;
        smallPharmaceuticalApparatusSaveDataGroup.everyTimeBurningMultiple = everyTimeBurningMultiple;
        smallPharmaceuticalApparatusSaveDataGroup.energyBurningTimeTask = energyBurningTimeTask;
        smallPharmaceuticalApparatusSaveDataGroup.energyBurningEndTimeTask = energyBurningEndTimeTask;
        if (waterTank.item == null)
        {
            smallPharmaceuticalApparatusSaveDataGroup.waterTank = null;
        }
        else
        {
            smallPharmaceuticalApparatusSaveDataGroup.waterTank = new ItemSlotSaveData() { itemId = waterTank.item.ItemId, itemSlotIndex = waterTank.itemIndex, attributeValueList = waterTank.item.attributeValueList };
        }

        if (produce.item == null)
        {
            smallPharmaceuticalApparatusSaveDataGroup.produce = null;
        }
        else
        {
            smallPharmaceuticalApparatusSaveDataGroup.produce = new ItemSlotSaveData() { itemId = produce.item.ItemId, itemSlotIndex = produce.itemIndex, attributeValueList = produce.item.attributeValueList };
        }

        if (energyTank.item == null)
        {
            smallPharmaceuticalApparatusSaveDataGroup.energyTank = null;
        }
        else
        {
            smallPharmaceuticalApparatusSaveDataGroup.energyTank = new ItemSlotSaveData() { itemId = energyTank.item.ItemId, itemSlotIndex = energyTank.itemIndex, attributeValueList = energyTank.item.attributeValueList };
        }

        smallPharmaceuticalApparatusSaveDataGroup.attributeCompositionDurationDict = attributeCompositionDurationDict;
        smallPharmaceuticalApparatusSaveDataGroup.combustionInterval = combustionInterval;
        smallPharmaceuticalApparatusSaveDataGroup.currentCombustionTime = currentCombustionTime;
        smallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData.Clear();

        foreach (ItemSlot slot in itemSlot)
        {
            if (slot.item != null)
            {
                smallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData.Add(new ItemSlotSaveData()
                {
                    itemId = slot.item.ItemId, itemSlotIndex = slot.itemIndex, attributeValueList = slot.item.attributeValueList
                });
            }
        }

        return smallPharmaceuticalApparatusSaveDataGroup;
    }


    private void Load()
    {
        SmallPharmaceuticalApparatusSaveDataGroup smallPharmaceuticalApparatusSaveDataGroup = ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.GetSmallPharmaceuticalApparatusSaveDataGroup(1);
        if (smallPharmaceuticalApparatusSaveDataGroup == null)
        {
            return;
        }

        foreach (ItemSlotSaveData itemSlotSaveData in smallPharmaceuticalApparatusSaveDataGroup.itemSlotSaveData)
        {
            Item item = ListenerFrameComponent.Instance.atlasSceneComponent.CreateItemByItemId(itemSlotSaveData.itemId);
            item.attributeValueList = itemSlotSaveData.attributeValueList;
            AddItem(item, itemSlotSaveData.itemSlotIndex);
        }

        //水容器
        if (smallPharmaceuticalApparatusSaveDataGroup.waterTank.itemSlotIndex != -1)
        {
            Item waterTankItem = ListenerFrameComponent.Instance.atlasSceneComponent.CreateItemByItemId(smallPharmaceuticalApparatusSaveDataGroup.waterTank.itemId);
            waterTankItem.attributeValueList = smallPharmaceuticalApparatusSaveDataGroup.waterTank.attributeValueList;
            _waterTank.AddItem(waterTankItem);
        }

        //能量容器
        if (smallPharmaceuticalApparatusSaveDataGroup.energyTank.itemSlotIndex != -1)
        {
            Item energyTankItem = ListenerFrameComponent.Instance.atlasSceneComponent.CreateItemByItemId(smallPharmaceuticalApparatusSaveDataGroup.energyTank.itemId);
            energyTankItem.attributeValueList = smallPharmaceuticalApparatusSaveDataGroup.energyTank.attributeValueList;
            _energyTank.AddItem(energyTankItem);
        }

        //输出容器
        if (smallPharmaceuticalApparatusSaveDataGroup.produce.itemSlotIndex != -1)
        {
            Item produceItem = ListenerFrameComponent.Instance.atlasSceneComponent.CreateItemByItemId(smallPharmaceuticalApparatusSaveDataGroup.produce.itemId);
            produceItem.attributeValueList = smallPharmaceuticalApparatusSaveDataGroup.produce.attributeValueList;
            _produce.AddItem(produceItem);
        }


        currentWater = smallPharmaceuticalApparatusSaveDataGroup.currentWater;
        incrementWater = smallPharmaceuticalApparatusSaveDataGroup.incrementWater;
        waterMax = smallPharmaceuticalApparatusSaveDataGroup.waterMax;
        temperatureMax = smallPharmaceuticalApparatusSaveDataGroup.temperatureMax;
        currentEnergy = smallPharmaceuticalApparatusSaveDataGroup.currentEnergy;
        temperatureConversionRatio = smallPharmaceuticalApparatusSaveDataGroup.temperatureConversionRatio;
        everyTimeBurningMultiple = smallPharmaceuticalApparatusSaveDataGroup.everyTimeBurningMultiple;
        _energyBurningTimeTask = smallPharmaceuticalApparatusSaveDataGroup.energyBurningTimeTask;
        _energyBurningEndTimeTask = smallPharmaceuticalApparatusSaveDataGroup.energyBurningEndTimeTask;
        attributeCompositionDurationDict = smallPharmaceuticalApparatusSaveDataGroup.attributeCompositionDurationDict;
        combustionInterval = smallPharmaceuticalApparatusSaveDataGroup.combustionInterval;
        currentCombustionTime = smallPharmaceuticalApparatusSaveDataGroup.currentCombustionTime;
        currentTemperature = smallPharmaceuticalApparatusSaveDataGroup.currentTemperature;
    }

    private bool AddItem(Item item, int itemSlotIndex)
    {
        foreach (ItemSlot itemSlot in _itemSlotContent)
        {
            if (itemSlot.itemIndex == itemSlotIndex)
            {
                if (itemSlot.IsNull())
                {
                    itemSlot.AddItem(item);
                    return true;
                }
            }
        }

        return false;
    }
}