//引入开始

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//引入结束
using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class PersonalBelongings : BaseWindow
{
    //变量声明开始
    private TextMeshProUGUI _maxCount;
    private TextMeshProUGUI _currentCount;
    private ScrollRect _itemSlotScrollView;
    private List<ItemSlot> _itemSlotContent;
    private Button _windowMoveEvent;

    private Button _close;

    //变量声明结束
    [SerializeField] [LabelText("移动布局")] private bool moveWindow;
    [LabelText("移动偏移")] private Vector3 _moveOffset;
    [LabelText("当前物品数量")] private int _currentItemCount = 0;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _maxCount, "MaxCount");
        BindUi(ref _currentCount, "MaxCount/CurrentCount");
        BindUi(ref _itemSlotScrollView, "ItemSlotScrollView");
        BindUi(ref _itemSlotContent, "ItemSlotScrollView/Viewport/ItemSlotContent");
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].ViewStartInit();
            _itemSlotContent[i].InitData(i);
        }

        BindUi(ref _windowMoveEvent, "Title/WindowMoveEvent");
        BindUi(ref _close, "Title/Close");
        //变量查找结束
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].InitScrollRect(_itemSlotScrollView);
            _itemSlotContent[i].PlaceItemEvent += ItemSlotPlaceItemEvent;
            _itemSlotContent[i].RemoveItemEvent += ItemSlotRemoveItemEvent;
        }
    }


    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);
        //变量绑定结束
        AddListenerEvent("InitStorageItem", InitStorageItem);
        AddReturnListenerEvent<Item, bool>("AddItem", AddItem);
        AddListenerEvent<ItemSlot>("SetDragItemSlot", SetDragItemSlot);
        AddListenerEvent("RemoveDragItemSlot", RemoveDragItemSlot);
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
        moveWindow = false;
        ListenerComponent.Instance.itemAttributeShow.SetWindowDrag(false);
    }

    private void OnCloseClick(BaseEventData targetObj)
    {
        HideThisView();
    }
    //变量方法结束

    //自定义属性开始

    //自定义属性结束
    protected override void Update()
    {
        base.Update();
        if (moveWindow)
        {
            window.transform.position = Input.mousePosition + _moveOffset;
        }
    }

    private void InitStorageItem()
    {
        ListenerComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataSaveEvent(Save);
        ListenerComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataLoadEvent(Load);
    }

    private void UpdateItemMaxCount()
    {
        _maxCount.text = "/" + _itemSlotContent.Count.ToString();
    }

    private void ItemSlotRemoveItemEvent()
    {
        _currentItemCount -= 1;
        _currentCount.text = _currentItemCount.ToString();
    }

    private void ItemSlotPlaceItemEvent(Item item)
    {
        _currentItemCount += 1;
        _currentCount.text = _currentItemCount.ToString();
    }

    [Button]
    private bool AddItem(Item item)
    {
        item = ScriptableObject.CreateInstance<Item>();
        foreach (ItemSlot itemSlot in _itemSlotContent)
        {
            if (itemSlot.IsNull())
            {
                itemSlot.AddItem(item);
                return true;
            }
        }

        return false;
    }

    private bool AddItem(Item item, int itemSlotIndex)
    {
        foreach (ItemSlot itemSlot in _itemSlotContent)
        {
            if (itemSlotIndex != -1 && itemSlot.itemIndex == itemSlotIndex)
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


    [LabelText("设置拖拽物品格子")]
    private void SetDragItemSlot(ItemSlot itemSlot)
    {
        ListenerComponent.Instance.tempDragItemSlot.SetDragItemSlot(itemSlot);
    }

    [LabelText("移除拖拽物品格子")]
    private void RemoveDragItemSlot()
    {
        ListenerComponent.Instance.tempDragItemSlot.RemoveDragItemSlot();
    }

    private void Save()
    {
        ListenerComponent.Instance.itemSlotDataSaveSceneComponent.SaveItemSlotSaveDataGroup(0, _itemSlotContent);
    }

    private void Load()
    {
        ItemSlotSaveDataGroup itemSlotSaveDataGroup = ListenerComponent.Instance.itemSlotDataSaveSceneComponent.GetItemSlotSaveDataGroup(0);
        if (itemSlotSaveDataGroup == null)
        {
            return;
        }

        foreach (ItemSlotSaveData itemSlotSaveData in itemSlotSaveDataGroup.itemSlotSaveData)
        {
            Item item = ListenerComponent.Instance.atlasSceneComponent.CreateItemByItemId(itemSlotSaveData.itemId);
            item.attributeValueList = itemSlotSaveData.attributeValueList;
            AddItem(item, itemSlotSaveData.itemSlotIndex);
        }

        UpdateItemMaxCount();
    }
}