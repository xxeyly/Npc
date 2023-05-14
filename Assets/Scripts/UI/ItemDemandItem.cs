#region 引入

using UnityEngine.UI;
using UnityEngine.EventSystems;

#endregion 引入

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemDemandItem : ChildBaseWindow
{
    #region 变量声明

    private Image _select;
    private Image _normal;
    private Image _null;
    private Image _itemIcon;
    private Text _content;
    private Image _add;
    private Button _event;

    #endregion 变量声明

    [LabelText("当前物品状态")] public ItemDemandItemType itemDemandItemType = ItemDemandItemType.无;

    [FormerlySerializedAs("DemandItem")] [LabelText("需求物品")]
    public Item demandItem;

    [LabelText("是否被选中")] public bool isSelect;

    public enum ItemDemandItemType
    {
        无,
        有
    }

    public override void Init()
    {
    }

    protected override void InitView()
    {
        #region 变量查找

        BindUi(ref _select, "Select");
        BindUi(ref _normal, "Normal");
        BindUi(ref _null, "Null");
        BindUi(ref _itemIcon, "ItemIcon");
        BindUi(ref _content, "Content");
        BindUi(ref _add, "Add");
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

        #endregion 变量绑定
    }


    #region 变量方法

    private void OnEventClick(BaseEventData targetObj)
    {
        if (itemDemandItemType == ItemDemandItemType.无)
        {
            //新增物品
        }
        else if (itemDemandItemType == ItemDemandItemType.有)
        {
            //编辑物品
        }
    }

    private void OnEventEnter(BaseEventData targetObj)
    {
        DisPlayObj(true, _select);
    }

    private void OnEventExit(BaseEventData targetObj)
    {
        if (!isSelect)
        {
            DisPlayObj(false, _select);
        }
    }

    private void OnEventDown(BaseEventData targetObj)
    {
        if (itemDemandItemType == ItemDemandItemType.无)
        {
            ListenerFrameComponent.Instance.purchaseItems.AddNewItem(itemIndex);
        }
        else
        {
            ListenerFrameComponent.Instance.purchaseItems.OnSelect(itemIndex);
        }
    }


    private void OnEventUp(BaseEventData targetObj)
    {
    }

    private void OnEventDrag(BaseEventData targetObj)
    {
    }

    private void OnEventBeginDrag(BaseEventData targetObj)
    {
    }

    private void OnEventEndDrag(BaseEventData targetObj)
    {
    }

    #endregion 变量方法

    public void ShowItem(Item item)
    {
        this.demandItem = item;
        _itemIcon.sprite = item.ItemIcon;
        DisPlayObj(false, _add, _null);
        DisPlayObj(true, _normal, _itemIcon);
        itemDemandItemType = ItemDemandItemType.有;
    }

    public void SetNull()
    {
        itemDemandItemType = ItemDemandItemType.无;
        DisPlayObj(true, _add, _null);
        DisPlayObj(false, _normal, _itemIcon);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        DisPlayObj(true, _select);
        isSelect = true;
    }

    public override void OnUnSelect()
    {
        base.OnUnSelect();
        DisPlayObj(false, _select);
        isSelect = false;
    }

    #region 自定义属性

    #endregion 自定义属性
}