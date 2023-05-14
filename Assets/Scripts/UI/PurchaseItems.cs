//引入开始

using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//引入结束
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class PurchaseItems : BaseWindow
{
    //变量声明开始
    private Button _windowMoveEvent;
    private Button _close;
    private ScrollRect _attributesDemand;
    private Button _remove;
    private ScrollRect _itemDemand;
    private List<ItemDemandItem> _itemSlotContent;

    private TextMeshProUGUI _tip;

    //变量声明结束
    [LabelText("属性预制体体")] public GameObject purchaseItemsRequiredAttributesObj;
    [LabelText("属性列表")] public List<PurchaseItemsRequiredAttributes> purchaseItemsRequiredAttributesList;
    [SerializeField] [LabelText("当前操作")] private int currentOperationItemDemandItem;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _windowMoveEvent, "Title/WindowMoveEvent");
        BindUi(ref _close, "Title/Close");
        BindUi(ref _attributesDemand, "AttributesDemand");
        BindUi(ref _remove, "AttributesDemand/Remove");
        BindUi(ref _itemDemand, "ItemDemand");
        BindUi(ref _itemSlotContent, "ItemDemand/Viewport/ItemSlotContent");
        for (int i = 0; i < _itemSlotContent.Count; i++)
        {
            _itemSlotContent[i].ViewStartInit();
            _itemSlotContent[i].InitData(i);
        }

        BindUi(ref _tip, "Tip");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);
        BindListener(_remove, EventTriggerType.PointerClick, OnRemoveClick);
        //变量绑定结束
        AddListenerEvent<int>("AddNewItem", AddNewItem);
        AddListenerEvent<int>("OnSelect", OnSelect);
    }

    //变量方法开始
    private void OnWindowMoveEventDown(BaseEventData targetObj)
    {
    }

    private void OnWindowMoveEventUp(BaseEventData targetObj)
    {
    }

    private void OnCloseClick(BaseEventData targetObj)
    {
        HideThisView();
    }

    private void OnRemoveClick(BaseEventData targetObj)
    {
    }
    //变量方法结束

    //自定义属性开始

    //自定义属性结束

    private void ShowItemAttributes(Item item)
    {
        //清空当前属性
        foreach (PurchaseItemsRequiredAttributes purchaseItemsRequiredAttributes in purchaseItemsRequiredAttributesList)
        {
            Destroy(purchaseItemsRequiredAttributes.gameObject);
        }
        //增加当前物品的属性
    }


    private void AddNewItem(int itemIndex)
    {
        currentOperationItemDemandItem = itemIndex;
        //新增物品
        HideThisView();
        ListenerFrameComponent.Instance.itemAtlasDisplay.SetSelectItemState(true);
        ListenerFrameComponent.Instance.itemAtlasDisplay.AddSelectItemDelegate(OnSelectItem);
        ShowView(typeof(ItemAtlasDisplay));
    }

    private void OnSelect(int itemIndex)
    {
        foreach (ItemDemandItem itemDemandItem in _itemSlotContent)
        {
            if (itemDemandItem.itemIndex == itemIndex)
            {
                itemDemandItem.OnSelect();
            }
            else
            {
                itemDemandItem.OnUnSelect();
            }
        }
    }

    private void OnSelectItem(Item item)
    {
        HideView(typeof(ItemAtlasDisplay));
        ShowThisView();
        ListenerFrameComponent.Instance.itemAtlasDisplay.SetSelectItemState(false);
        ListenerFrameComponent.Instance.itemAtlasDisplay.RemoveSelectItemDelegate(OnSelectItem);
        if (item != null)
        {
        }

        ItemDemandItem tempItemDemandItem = null;

        foreach (ItemDemandItem itemDemandItem in _itemSlotContent)
        {
            if (itemDemandItem.itemIndex == currentOperationItemDemandItem)
            {
                tempItemDemandItem = itemDemandItem;
            }
        }

        if (tempItemDemandItem != null)
        {
            tempItemDemandItem.ShowItem(item);
            OnSelect(tempItemDemandItem.itemIndex);
        }
    }
}