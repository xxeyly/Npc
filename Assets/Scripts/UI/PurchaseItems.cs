#region 引入

using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

#endregion 引入

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class PurchaseItems : BaseWindow
{
    #region 变量声明

    private Button _windowMoveEvent;
    private Button _close;
    private ScrollRect _itemDemand;
    private List<ItemDemandItem> _itemSlotContent;
    private GameObject _purchaseData;
    private GameObject _tip;
    private ScrollRect _attributesDemand;
    private GameObject _purchaseItemsRequiredAttributesContent;
    private ScrollRect _exchangeItems;
    private List<ItemSlot> _exchangeItemsContent;
    private Button _remove;

    #endregion 变量声明

    [LabelText("属性预制体体")] public GameObject purchaseItemsRequiredAttributesObj;
    [LabelText("属性列表")] public List<PurchaseItemsRequiredAttributes> purchaseItemsRequiredAttributesList;
    [SerializeField] [LabelText("当前操作")] private ItemDemandItem currentOperationItemDemandItem;
    [LabelText("收购物品数据")] [SerializeField] private Dictionary<ItemDemandItem, ItemDemandItemData> itemDemandItemDataDic = new Dictionary<ItemDemandItem, ItemDemandItemData>();

    [BoxGroup("界面移动")] [SerializeField] [LabelText("移动布局")]
    private bool moveWindow;

    [BoxGroup("界面移动")] [LabelText("移动偏移")] private Vector3 _moveOffset;

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

        BindUi(ref _purchaseData, "PurchaseData");
        BindUi(ref _tip, "PurchaseData/Tip");
        BindUi(ref _attributesDemand, "PurchaseData/AttributesDemand");
        BindUi(ref _purchaseItemsRequiredAttributesContent, "PurchaseData/AttributesDemand/Viewport/PurchaseItemsRequiredAttributesContent");
        BindUi(ref _exchangeItems, "PurchaseData/ExchangeItems");
        BindUi(ref _exchangeItemsContent, "PurchaseData/ExchangeItems/Viewport/ExchangeItemsContent");
        for (int i = 0; i < _exchangeItemsContent.Count; i++)
        {
            _exchangeItemsContent[i].ViewStartInit();
            _exchangeItemsContent[i].InitData(i);
        }

        BindUi(ref _remove, "PurchaseData/Remove");

        #endregion 变量查找

        for (int i = 0; i < _exchangeItemsContent.Count; i++)
        {
            _exchangeItemsContent[i].PlaceItemEvent += OnPlaceItemEvent;
            _exchangeItemsContent[i].RemoveItemEvent += OnRemoveItemEvent;
        }
    }

    private void OnRemoveItemEvent(ItemSlot itemSlot, Item item)
    {
        Debug.Log(itemSlot.name + "移除物品:" + item.ItemName);
    }

    private void OnPlaceItemEvent(ItemSlot itemSlot, Item item)
    {
        Debug.Log(itemSlot.name + "增加物品:" + item.ItemName);
    }

    protected override void InitListener()
    {
        #region 变量绑定

        BindListener(_windowMoveEvent, EventTriggerType.PointerDown, OnWindowMoveEventDown);
        BindListener(_windowMoveEvent, EventTriggerType.PointerUp, OnWindowMoveEventUp);
        BindListener(_close, EventTriggerType.PointerClick, OnCloseClick);
        BindListener(_remove, EventTriggerType.PointerClick, OnRemoveClick);

        #endregion 变量绑定
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

    private void OnRemoveClick(BaseEventData targetObj)
    {
        if (currentOperationItemDemandItem == null)
        {
            return;
        }

        DisPlayObj(false, _attributesDemand.gameObject);
        DisPlayObj(true, _tip);
        //设置为空
        currentOperationItemDemandItem.SetNull();
        if (itemDemandItemDataDic.ContainsKey(currentOperationItemDemandItem))
        {
            itemDemandItemDataDic.Remove(currentOperationItemDemandItem);
        }
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

    //显示当前物品的所有属性数据
    private void ShowItemAttributes()
    {
        DisPlayObj(true, _attributesDemand.gameObject, _exchangeItems.gameObject, _remove.gameObject);
        DisPlayObj(false, _tip.gameObject);
        //清空当前属性
        foreach (PurchaseItemsRequiredAttributes purchaseItemsRequiredAttributes in purchaseItemsRequiredAttributesList)
        {
            DestroyImmediate(purchaseItemsRequiredAttributes.gameObject);
        }

        purchaseItemsRequiredAttributesList.Clear();
        //增加当前物品的属性
        //获得当前属性
        if (itemDemandItemDataDic.ContainsKey(currentOperationItemDemandItem))
        {
            ItemDemandItemData itemDemandItemData = itemDemandItemDataDic[currentOperationItemDemandItem];
            foreach (AttributeValue attributeValue in currentOperationItemDemandItem.demandItem.attributeValueList)
            {
                GameObject requiredAttributesObj = Instantiate(purchaseItemsRequiredAttributesObj, _purchaseItemsRequiredAttributesContent.transform);
                PurchaseItemsRequiredAttributes tempPurchaseItemsRequiredAttributes = requiredAttributesObj.GetComponent<PurchaseItemsRequiredAttributes>();
                tempPurchaseItemsRequiredAttributes.ViewStartInit();
                tempPurchaseItemsRequiredAttributes.SetAttributes(currentOperationItemDemandItem, attributeValue, itemDemandItemData.attributeValue[attributeValue]);
                purchaseItemsRequiredAttributesList.Add(tempPurchaseItemsRequiredAttributes);
            }
        }
        else
        {
            Debug.Log("程序错误");
        }
    }

    [LabelText("新增需求物品")]
    [AddListenerEvent]
    private void AddNewItem(int itemIndex)
    {
        foreach (ItemDemandItem itemDemandItem in _itemSlotContent)
        {
            if (itemDemandItem.itemIndex == itemIndex)
            {
                currentOperationItemDemandItem = itemDemandItem;
                break;
            }
        }

        //新增物品
        HideThisView();
        ListenerFrameComponent.Instance.itemAtlasDisplay.SetSelectItemState(true);
        ListenerFrameComponent.Instance.itemAtlasDisplay.AddSelectItemDelegate(OnReceiveAtlasItem);
        ShowView(typeof(ItemAtlasDisplay));
    }

    [LabelText("选中已有物品")]
    [AddListenerEvent]
    private void OnSelect(int itemIndex)
    {
        foreach (ItemDemandItem itemDemandItem in _itemSlotContent)
        {
            if (itemDemandItem.itemIndex == itemIndex)
            {
                currentOperationItemDemandItem = itemDemandItem;
                itemDemandItem.OnSelect();
            }
            else
            {
                itemDemandItem.OnUnSelect();
            }
        }

        ShowItemAttributes();
    }

    [LabelText("接收图鉴物品")]
    private void OnReceiveAtlasItem(Item item)
    {
        HideView(typeof(ItemAtlasDisplay));
        ShowThisView();
        ListenerFrameComponent.Instance.itemAtlasDisplay.SetSelectItemState(false);
        ListenerFrameComponent.Instance.itemAtlasDisplay.RemoveSelectItemDelegate(OnReceiveAtlasItem);
        //记录新物品数据
        if (!itemDemandItemDataDic.ContainsKey(currentOperationItemDemandItem))
        {
            ItemDemandItemData itemDemandItemData = new ItemDemandItemData();
            Dictionary<AttributeValue, Vector2> tempAttributeValueData = new Dictionary<AttributeValue, Vector2>();

            foreach (AttributeValue attributeValue in item.attributeValueList)
            {
                tempAttributeValueData.Add(attributeValue, Vector2.zero);
            }

            itemDemandItemData.attributeValue = tempAttributeValueData;

            itemDemandItemDataDic.Add(currentOperationItemDemandItem, itemDemandItemData);
        }
        else
        {
            Debug.Log("程序错误");
        }

        //显示UI
        currentOperationItemDemandItem.ShowItem(item);
        //选择当前物品
        OnSelect(currentOperationItemDemandItem.itemIndex);
    }

    [LabelText("修改属性值")]
    [AddListenerEvent]
    private void OnEditorAttributeValue(ItemDemandItem itemDemandItem, AttributeValue attributeValue, Vector2 value)
    {
        itemDemandItemDataDic[itemDemandItem].attributeValue[attributeValue] = value;
    }

    [Serializable]
    [LabelText("收购数据")]
    public class ItemDemandItemData
    {
        [LabelText("所需物品属性")] public Dictionary<AttributeValue, Vector2> attributeValue = new Dictionary<AttributeValue, Vector2>();
        [LabelText("交换物品")] private List<Item> exchangeItems = new List<Item>();
    }
}