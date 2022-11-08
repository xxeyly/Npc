//引入开始

using UnityEngine.UI;
using UnityEngine.EventSystems;
//引入结束
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

public class TempDragItemSlot : BaseWindow
{
    //变量声明开始
    private GameObject _itemSlot;
    private Image _normal;
    private Image _null;
    private Text _content;

    private Image _itemIcon;

    //变量声明结束
    [LabelText("拖拽物品格子")] private Item _dragItem;
    [LabelText("偏移值")] [SerializeField] private Vector2 offsetValue;
    [LabelText("拖拽中")] [SerializeField] private bool drag;

    [LabelText("按下的物品格子")] [SerializeField]
    private ItemSlot downItemSlot;

    [LabelText("进入的物品格子")] [SerializeField]
    private ItemSlot enterItemSlot;


    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _itemSlot, "ItemSlot");
        BindUi(ref _normal, "ItemSlot/Normal");
        BindUi(ref _null, "ItemSlot/Null");
        BindUi(ref _content, "ItemSlot/Content");
        BindUi(ref _itemIcon, "ItemSlot/ItemIcon");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始

        //变量绑定结束
        AddListenerEvent<ItemSlot>("SetDragItemSlot", SetDragItemSlot);
        AddListenerEvent("RemoveDragItemSlot", RemoveDragItemSlot);
        AddListenerEvent<ItemSlot>("SetEnterItemSlot", SetEnterItemSlot);
        AddListenerEvent("SetEnterItemSlotNull", SetEnterItemSlotNull);

        AddReturnListenerEvent<bool>("GetDragSate", GetDragSate);
    }


    //变量方法开始

    //变量方法结束

    //自定义属性开始

    //自定义属性结束

    protected override void Update()
    {
        base.Update();
        if (drag)
        {
            transform.position = Input.mousePosition;
        }
    }

    [LabelText("拖拽状态")]
    private bool GetDragSate()
    {
        return drag;
    }

    [LabelText("设置拖拽物品格子")]
    private void SetDragItemSlot(ItemSlot itemSlot)
    {
        if (_dragItem == null)
        {
            _dragItem = itemSlot.item;
        }

        //设置按下的物品格子
        downItemSlot = itemSlot;
        enterItemSlot = itemSlot;
        drag = true;
        UpdateItemUp();
        DisPlayObj(true, _itemSlot);
    }

    [LabelText("设置进入物品格子")]
    private void SetEnterItemSlot(ItemSlot itemSlot)
    {
        enterItemSlot = itemSlot;
    }

    [LabelText("设置进入物品格子空")]
    private void SetEnterItemSlotNull()
    {
        enterItemSlot = null;
    }

    private void UpdateItemUp()
    {
        _itemIcon.sprite = _dragItem.ItemIcon;
        if (ListenerComponent.Instance.atlasSceneComponent.GetItemUnlocking(_dragItem))
        {
            _content.text = _dragItem.ItemName;
        }
        else
        {
            _content.text = "未知物品";
        }
    }

    [LabelText("移除拖拽物品格子")]
    private void RemoveDragItemSlot()
    {
        drag = false;
        DisPlayObj(false, _itemSlot);
        if (enterItemSlot == null)
        {
            downItemSlot.AddItem(_dragItem);
        }
        else
        {
            //格子物品限制
            if (enterItemSlot.restricted)
            {
                //检测合格,可以放置
                if (enterItemSlot.CheckItemQualified(_dragItem))
                {
                    //移动到的格子为空,放置物品
                    if (enterItemSlot.item == null)
                    {
                        enterItemSlot.AddItem(_dragItem);
                        enterItemSlot.ShowItemAttribute();
                    }
                    //交换物品
                    else
                    {
                        downItemSlot.AddItem(enterItemSlot.item);
                        enterItemSlot.AddItem(_dragItem);
                        enterItemSlot.ShowItemAttribute();
                    }
                }
                else
                {
                    //退回物品
                    downItemSlot.AddItem(_dragItem);
                }
            }
            else
            {
                //移动到的格子为空,放置物品
                if (enterItemSlot.item == null)
                {
                    enterItemSlot.AddItem(_dragItem);
                    enterItemSlot.ShowItemAttribute();
                }
                //交换物品
                else
                {
                    downItemSlot.AddItem(enterItemSlot.item);
                    enterItemSlot.AddItem(_dragItem);
                    enterItemSlot.ShowItemAttribute();
                }
            }
        }


        downItemSlot = null;
        enterItemSlot = null;
        _dragItem = null;
    }
}