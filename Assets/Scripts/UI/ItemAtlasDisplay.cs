#region 引入

using UnityEngine.UI;
using UnityEngine.EventSystems;

#endregion 引入

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using XFramework;

public delegate void SelectItemDelegate(Item item);

public class ItemAtlasDisplay : BaseWindow
{
    #region 变量声明

    private Button _windowMoveEvent;
    private Button _close;
    private ScrollRect _itemDemand;

    private List<ItemAtlasDisplayItem> _itemSlotContent;

    #endregion 变量声明

    private Transform _itemSlotContentPatient;
    [SerializeField] [LabelText("图鉴预制体")] private GameObject itemAtlasDisplayItemPre;
    [SerializeField] [LabelText("选择物品")] private bool selectItemState;
    [SerializeField] private SelectItemDelegate selectItemDelegate;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        #region 变量查找

        BindUi(ref _windowMoveEvent, "Title/WindowMoveEvent");
        BindUi(ref _close, "Title/Close");
        BindUi(ref _itemDemand, "ItemDemand");
        BindUi(ref _itemSlotContent, "ItemDemand/Viewport/ItemSlotContent");
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].ViewStartInit();
            _itemSlotContent[i].InitData(i);
        }

        #endregion 变量查找

        BindUi(ref _itemSlotContentPatient, "ItemDemand/Viewport/ItemSlotContent");
    }

    protected override void InitListener()
    {
        #region 变量绑定

        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);

        #endregion 变量绑定
       
    }

    #region 变量方法

    private void OnWindowMoveEventDown(BaseEventData targetObj)
    {
    }

    private void OnWindowMoveEventUp(BaseEventData targetObj)
    {
    }

    private void OnCloseClick(BaseEventData targetObj)
    {
    }

    #endregion 变量方法

    #region 自定义属性

    #endregion 自定义属性

    [AddListenerEvent]
    private void SetSelectItemState(bool value)
    {
        selectItemState = value;
    }

    [AddListenerEvent]
    private void OnSelect(int itemIndex)
    {
        foreach (Item item in ListenerFrameComponent.Instance.atlasSceneComponent.GetItemAtlas().Items)
        {
            if (item.ItemId == itemIndex)
            {
                selectItemDelegate?.Invoke(item);
                return;
            }
        }
    }

    [AddListenerEvent]
    private void OnEnter(int itemId)
    {
        if (!selectItemState)
        {
            return;
        }

        foreach (ItemAtlasDisplayItem itemAtlasDisplayItem in _itemSlotContent)
        {
            if (itemAtlasDisplayItem.lockState && itemAtlasDisplayItem.itemIndex == itemId)
            {
                itemAtlasDisplayItem.OnSelect();
            }
            else
            {
                itemAtlasDisplayItem.OnUnSelect();
            }
        }
    }

    [AddListenerEvent]
    private void InitAtlas()
    {
        List<Item> itemAtlas = ListenerFrameComponent.Instance.atlasSceneComponent.GetItemAtlas().Items;
        List<Item> unlockingItem = ListenerFrameComponent.Instance.atlasSceneComponent.GetUnlockingItem();

        foreach (Item item in itemAtlas)
        {
            bool unlockingState = false;
            foreach (Item unlocking in unlockingItem)
            {
                if (unlocking.ItemId == item.ItemId)
                {
                    unlockingState = true;
                }
            }

            GameObject itemAtlasDisplayItem = Instantiate(itemAtlasDisplayItemPre);
            itemAtlasDisplayItem.transform.SetParent(_itemSlotContentPatient);
            ItemAtlasDisplayItem tempItemAtlasDisplayItem = itemAtlasDisplayItem.GetComponent<ItemAtlasDisplayItem>();
            tempItemAtlasDisplayItem.ViewStartInit();
            tempItemAtlasDisplayItem.SetLockState(unlockingState);
            tempItemAtlasDisplayItem.InitData(item.ItemId);
            tempItemAtlasDisplayItem.InitData(item);
            _itemSlotContent.Add(tempItemAtlasDisplayItem);
        }
    }

    [AddListenerEvent]
    private void AddSelectItemDelegate(SelectItemDelegate action)
    {
        selectItemDelegate += action;
    }

    [AddListenerEvent]
    private void RemoveSelectItemDelegate(SelectItemDelegate action)
    {
        selectItemDelegate -= action;
    }
}