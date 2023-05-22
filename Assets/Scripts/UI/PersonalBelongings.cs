#region 引入

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
#endregion 引入
using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class PersonalBelongings : BaseWindow
{
    #region 变量声明
    private TextMeshProUGUI _maxCount;
    private TextMeshProUGUI _currentCount;
    private ScrollRect _itemSlotScrollView;
    private List<ItemSlot> _itemSlotContent;
    private Button _windowMoveEvent;

    private Button _close;

    #endregion 变量声明
    [SerializeField] [LabelText("移动布局")] private bool moveWindow;
    [LabelText("移动偏移")] private Vector3 _moveOffset;
    [LabelText("当前物品数量")] private int _currentItemCount = 0;

    public override void Init()
    {
    }

    protected override void InitView()
    {
       #region 变量查找
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
        #endregion 变量查找
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].InitScrollRect(_itemSlotScrollView);
            _itemSlotContent[i].PlaceItemEvent += ItemSlotPlaceItemEvent;
            _itemSlotContent[i].RemoveItemEvent += ItemSlotRemoveItemEvent;
        }
    }


    protected override void InitListener()
    {
        #region 变量绑定
        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);
        #endregion 变量绑定
        AddListenerEvent("InitStorageItem", InitStorageItem);
        AddReturnListenerEvent<Item, bool>("AddItem", AddItem);
        AddListenerEvent<ItemSlot>("SetDragItemSlot", SetDragItemSlot);
        AddListenerEvent("RemoveDragItemSlot", RemoveDragItemSlot);
    }

    #region 变量方法
    private void OnWindowMoveEventDown(BaseEventData targetObj)
    {
        ListenerFrameComponent.Instance.itemAttributeShow.HideItemAttribute();
        ListenerFrameComponent.Instance.itemAttributeShow.SetWindowDrag(true);
        _moveOffset = window.transform.position - Input.mousePosition;
        moveWindow = true;
    }

    private void OnWindowMoveEventUp(BaseEventData targetObj)
    {
        moveWindow = false;
        ListenerFrameComponent.Instance.itemAttributeShow.SetWindowDrag(false);
    }

    private void OnCloseClick(BaseEventData targetObj)
    {
        HideThisView();
    }
    #endregion 变量方法

    #region 自定义属性

    #endregion 自定义属性
    protected override void Update()
    {
        base.Update();
        if (moveWindow)
        {
            window.transform.position = Input.mousePosition + _moveOffset;
        }
    }
    [AddListenerEvent]
    private void InitStorageItem()
    {
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataSaveEvent(Save);
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SetItemSlotDataLoadEvent(Load);
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

    [Button][AddListenerEvent]
    private bool AddItem(Item item)
    {
        item = item.GetNewItem();
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


    [LabelText("设置拖拽物品格子")][AddListenerEvent]
    private void SetDragItemSlot(ItemSlot itemSlot)
    {
        ListenerFrameComponent.Instance.tempDragItemSlot.SetDragItemSlot(itemSlot);
    }

    [LabelText("移除拖拽物品格子")][AddListenerEvent]
    private void RemoveDragItemSlot()
    {
        ListenerFrameComponent.Instance.tempDragItemSlot.RemoveDragItemSlot();
    }

    private void Save()
    {
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.SaveItemSlotSaveDataGroup(0, _itemSlotContent);
    }

    private void Load()
    {
        ItemSlotSaveDataGroup itemSlotSaveDataGroup = ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.GetItemSlotSaveDataGroup(0);
        if (itemSlotSaveDataGroup == null)
        {
            return;
        }

        foreach (ItemSlotSaveData itemSlotSaveData in itemSlotSaveDataGroup.itemSlotSaveData)
        {
            Item item = ListenerFrameComponent.Instance.atlasSceneComponent.CreateItemByItemId(itemSlotSaveData.itemId);
            item.attributeValueList = itemSlotSaveData.attributeValueList;
            AddItem(item, itemSlotSaveData.itemSlotIndex);
        }

        UpdateItemMaxCount();
    }
}