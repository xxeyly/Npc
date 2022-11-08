//引入开始

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//引入结束
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemSlot : ChildBaseWindow
{
    //变量声明开始
    private Image _normal;
    private Image _null;
    private Image _itemIcon;
    private Text _content;

    private Button _event;

    //变量声明结束
    [LabelText("物品属性")] public Item item;
    [LabelText("限制")] public bool restricted;
    [LabelText("限制物品列表")] public List<Item> restrictedItemList = new List<Item>();
    private ScrollRect _scrollRect;
    private bool _drag;

    public delegate void PlaceItem(Item item);

    public PlaceItem PlaceItemEvent;

    public delegate void RemoveItem();

    public RemoveItem RemoveItemEvent;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _normal, "Normal");
        BindUi(ref _null, "Null");
        BindUi(ref _itemIcon, "ItemIcon");
        BindUi(ref _content, "Content");
        BindUi(ref _event, "Event");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_event, EventTriggerType.PointerClick, OnEventClick);
        BindListener(_event, EventTriggerType.PointerEnter, OnEventEnter);
        BindListener(_event, EventTriggerType.PointerExit, OnEventExit);
        BindListener(_event, EventTriggerType.PointerDown, OnEventDown);
        BindListener(_event, EventTriggerType.PointerUp, OnEventUp);
        BindListener(_event, EventTriggerType.Drag, OnEventDrag);
        BindListener(_event, EventTriggerType.BeginDrag, OnEventBeginDrag);
        BindListener(_event, EventTriggerType.EndDrag, OnEventEndDrag);
        BindListener(_event, EventTriggerType.Scroll, OnEventScroll);
        //变量绑定结束
    }


    //变量方法开始
    private void OnEventClick(BaseEventData targetObj)
    {
        if (_drag)
        {
            return;
        }
    }

    private void OnEventEnter(BaseEventData targetObj)
    {
        if (ListenerComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerComponent.Instance.tempDragItemSlot.SetEnterItemSlot(this);
        }

        ShowItemAttribute();
    }

    private void OnEventExit(BaseEventData targetObj)
    {
        if (ListenerComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerComponent.Instance.tempDragItemSlot.SetEnterItemSlotNull();
        }

        ListenerComponent.Instance.itemAttributeShow.HideItemAttribute();
    }

    private void OnEventDown(BaseEventData targetObj)
    {
        if (item == null)
        {
            return;
        }


        ListenerComponent.Instance.personalBelongings.SetDragItemSlot(this);
        item = null;
        UpdateItemUI();
        RemoveItemEvent?.Invoke();
    }

    private void OnEventUp(BaseEventData targetObj)
    {
        if (ListenerComponent.Instance.tempDragItemSlot.GetDragSate())
        {
            ListenerComponent.Instance.tempDragItemSlot.RemoveDragItemSlot();
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

    //变量方法结束
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
            ListenerComponent.Instance.itemAttributeShow.ShowItemAttribute(item, transform.position + new Vector3(152 / 2f, 152 / 2f));
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
            RemoveItemEvent?.Invoke();
        }

        this.item = item;
        UpdateItemUI();
        PlaceItemEvent?.Invoke(this.item);
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
            if (ListenerComponent.Instance.atlasSceneComponent.GetItemUnlocking(item))
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