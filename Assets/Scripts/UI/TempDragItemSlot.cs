#region 引入

using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion 引入
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

public class TempDragItemSlot : BaseWindow
{
    #region 变量声明
    private GameObject _itemSlot;
    private Image _normal;
    private Image _null;
    private Text _content;

    private Image _itemIcon;

    #endregion 变量声明
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
       #region 变量查找
        BindUi(ref _itemSlot, "ItemSlot");
        BindUi(ref _normal, "ItemSlot/Normal");
        BindUi(ref _null, "ItemSlot/Null");
        BindUi(ref _content, "ItemSlot/Content");
        BindUi(ref _itemIcon, "ItemSlot/ItemIcon");
        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定

        #endregion 变量绑定
        AddListenerEvent<ItemSlot>("SetDragItemSlot", SetDragItemSlot);
        AddListenerEvent("RemoveDragItemSlot", RemoveDragItemSlot);
        AddListenerEvent<ItemSlot>("SetEnterItemSlot", SetEnterItemSlot);
        AddListenerEvent("SetEnterItemSlotNull", SetEnterItemSlotNull);

        AddReturnListenerEvent<bool>("GetDragSate", GetDragSate);
    }


    #region 变量方法

    #endregion 变量方法

    #region 自定义属性

    #endregion 自定义属性

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
        if (ListenerFrameComponent.Instance.atlasSceneComponent.GetItemUnlocking(_dragItem))
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