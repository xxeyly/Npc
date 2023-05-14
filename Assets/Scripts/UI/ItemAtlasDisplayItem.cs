//引入开始

using UnityEngine.UI;
using UnityEngine.EventSystems;
//引入结束
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemAtlasDisplayItem : ChildBaseWindow
{
    //变量声明开始
    private Image _normal;
    private Image _null;
    private Image _itemIcon;
    private Text _content;
    private Image _select;
    private Image _lock;

    private Button _event;

    //变量声明结束
    [SerializeField] [LabelText("解锁状态")] public bool lockState;

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
        BindUi(ref _select, "Select");
        BindUi(ref _lock, "Lock");
        BindUi(ref _event, "Event");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_event, EventTriggerType.PointerClick, OnEventClick);
        BindListener(_event, EventTriggerType.PointerEnter, OnEventEnter);
        BindListener(_event, EventTriggerType.PointerExit, OnEventExit);
        //变量绑定结束
    }


    //变量方法开始
    private void OnEventClick(BaseEventData targetObj)
    {
        if (lockState)
        {
            ListenerFrameComponent.Instance.itemAtlasDisplay.OnSelect(itemIndex);
        }
    }

    private void OnEventEnter(BaseEventData targetObj)
    {
        ListenerFrameComponent.Instance.itemAtlasDisplay.OnEnter(itemIndex);
    }

    private void OnEventExit(BaseEventData targetObj)
    {
    }
    //变量方法结束

    public void SetLockState(bool lockState)
    {
        DisPlayObj(!lockState, _lock);
        DisPlayObj(lockState, _itemIcon);
        this.lockState = lockState;
    }

    public void InitData(Item item)
    {
        if (lockState)
        {
            _itemIcon.sprite = item.ItemIcon;
            _content.text = item.ItemName;
        }
    }

    public override void OnSelect()
    {
        base.OnSelect();
        if (lockState)
        {
            DisPlayObj(true, _select);
        }
    }

    public override void OnUnSelect()
    {
        base.OnUnSelect();
        if (lockState)
        {
            DisPlayObj(false, _select);
        }
    }
}