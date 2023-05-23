#region 引入

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#endregion 引入

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemSlot : ChildBaseWindow
{
    #region 变量声明

    private Image _normal;
    private Image _null;
    private Image _itemIcon;
    private Text _content;

    private Button _event;

    #endregion 变量声明

    [LabelText("物品属性")] public Item item;
    [LabelText("限制")] public bool restricted;
    [LabelText("限制物品列表")] public List<Item> restrictedItemList = new List<Item>();
    private ScrollRect _scrollRect;
    private bool _drag;

    public delegate void PlaceItem(ItemSlot itemSlot, Item item);

    public PlaceItem PlaceItemEvent;

    public delegate void RemoveItem(ItemSlot itemSlot, Item item);

    public RemoveItem RemoveItemEvent;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        #region 变量查找

        BindUi(ref _normal, "Normal");
        BindUi(ref _null, "Null");
        BindUi(ref _itemIcon, "ItemIcon");
        BindUi(ref _content, "Content");
        BindUi(ref _event, "Event");

        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定

        BindListener(_event, EventTriggerType.PointerClick, OnEventClick);
        BindListener(_event, EventTriggerType.PointerEnter, OnEventEnter);
        BindListener(_event, EventTriggerType.PointerExit, OnEventExit);
        BindListener(_event, EventTriggerType.PointerDown, OnEventDown);
        BindListener(_event, EventTriggerType.PointerUp, OnEventUp);
        BindListener(_event, EventTriggerType.Drag, OnEventDrag);
        BindListener(_event, EventTriggerType.BeginDrag, OnEventBeginDrag);
        BindListener(_event, EventTriggerType.EndDrag, OnEventEndDrag);
        BindListener(_event, EventTriggerType.Scroll, OnEventScroll);

        #endregion 变量绑定
    }


    #region 变量方法

    private void OnEventClick(BaseEventData targetObj)
    {
    }

    private void OnEventEnter(BaseEventData targetObj)
    {
        if (ListenerFrameComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerFrameComponent.Instance.tempDragItemSlot.SetEnterItemSlot(this);
        }

        ShowItemAttribute();
    }

    private void OnEventExit(BaseEventData targetObj)
    {
        if (ListenerFrameComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerFrameComponent.Instance.tempDragItemSlot.SetEnterItemSlotNull();
        }

        ListenerFrameComponent.Instance.itemAttributeShow.HideItemAttribute();
    }

    private void OnEventDown(BaseEventData targetObj)
    {
        if (item == null)
        {
            return;
        }


        ListenerFrameComponent.Instance.personalBelongings.SetDragItemSlot(this);
        RemoveItemEvent?.Invoke(this, item);
        item = null;
        UpdateItemUI();
    }

    private void OnEventUp(BaseEventData targetObj)
    {
        if (ListenerFrameComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerFrameComponent.Instance.tempDragItemSlot.RemoveDragItemSlot();
        }
    }

    private void OnEventDrag(BaseEventData targetObj)
    {
        // _scrollRect.OnDrag((PointerEventData)targetObj);
    }

    private void OnEventBeginDrag(BaseEventData targetObj)
    {
        _drag = true;
        // _scrollRect.OnBeginDrag((PointerEventData)targetObj);
    }

    private void OnEventEndDrag(BaseEventData targetObj)
    {
        _drag = false;
        // _scrollRect.OnEndDrag((PointerEventData)targetObj);
    }

    private void OnEventScroll(BaseEventData targetObj)
    {
        if (_scrollRect == null)
        {
            return;
        }

        _scrollRect.OnScroll((PointerEventData)targetObj);
    }

    #endregion 变量方法

    [LabelText("检测物品是否合格")]
    public bool CheckItemQualified(Item item)
    {
        foreach (Item restrictedItem in restrictedItemList)
        {
            if (restrictedItem.ItemId == item.ItemId)
            {
                return true;
            }
        }

        return false;
    }

    public void ShowItemAttribute()
    {
        if (item != null)
        {
            ListenerFrameComponent.Instance.itemAttributeShow.ShowItemAttribute(item, transform.position + new Vector3(152 / 2f, 152 / 2f));
        }
    }

    public bool IsNull()
    {
        return item == null;
    }

    public void AddItem(Item item)
    {
        if (this.item != null)
        {
            RemoveItemEvent?.Invoke(this, item);
        }

        this.item = item;
        UpdateItemUI();
        PlaceItemEvent?.Invoke(this, this.item);
    }

    //更新UI
    public void UpdateItemUI()
    {
        if (item == null)
        {
            DisPlayObj(false, _normal, _itemIcon, _content);
            DisPlayObj(true, _null);
        }
        else
        {
            DisPlayObj(true, _normal, _itemIcon, _content);
            DisPlayObj(false, _null);
            _itemIcon.sprite = item.ItemIcon;
            if (ListenerFrameComponent.Instance.atlasSceneComponent.GetItemUnlocking(item))
            {
                _content.text = item.ItemName;
            }
            else
            {
                _content.text = "未知物品";
            }
        }
    }

    public void InitScrollRect(ScrollRect scrollRect)
    {
        this._scrollRect = scrollRect;
    }
}