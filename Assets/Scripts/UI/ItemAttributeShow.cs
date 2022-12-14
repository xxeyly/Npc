//引入开始

using UnityEngine.UI;
using TMPro;
//引入结束
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

public class ItemAttributeShow : BaseWindow
{
    //变量声明开始
    private GameObject _attributePanel;
    private Image _itemIcon;
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _describe;
    private Image _attributeTitle;

    private GameObject _itemAttributeShowItemPanel;

    //变量声明结束
    private RectTransform _attributePanelRect;
    private RectTransform _describeRect;
    private RectTransform _attributeTitleRect;
    private RectTransform _itemAttributeShowItemPanelRect;

    [LabelText("物品图片占用大小")] public float itemIconSize;
    [LabelText("物品图片和描述的间距")] public float itemIconSSpacingDescribe;
    [LabelText("描述和属性标题的间距")] public float describeSpacingAttributeTitle;
    [LabelText("属性和属性类表的间距")] public float attributeTitleSpacingItemAttributeShowItemPanel;
    [LabelText("属性预制体")] public GameObject itemAttributeShowItemPrefab;
    [LabelText("预制体列表")] private List<ItemAttributeShowItem> _itemAttributeShowItems = new List<ItemAttributeShowItem>();
    private Transform _itemAttributeShowItemContent;
    [LabelText("刷新布局")] private bool _refreshLayout;
    private Vector2 _itemPos;
    [LabelText("偏移")] public Vector2 offset;
    private int _updateAttributePanelSizeTimeTask;
    [LabelText("窗口拖拽中")] private bool _windowDrag;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _attributePanel, "AttributePanel");
        BindUi(ref _itemIcon, "AttributePanel/SelectedFrame/ItemIcon");
        BindUi(ref _itemName, "AttributePanel/ItemName");
        BindUi(ref _describe, "AttributePanel/Describe");
        BindUi(ref _attributeTitle, "AttributePanel/AttributeTitle");
        BindUi(ref _itemAttributeShowItemPanel, "AttributePanel/ItemAttributeShowItemPanel");
        //变量查找结束
        BindUi(ref _itemAttributeShowItemContent, "AttributePanel/ItemAttributeShowItemPanel");
        BindUi(ref _attributePanelRect, "AttributePanel");
        BindUi(ref _describeRect, "AttributePanel/Describe");
        BindUi(ref _attributeTitleRect, "AttributePanel/AttributeTitle");
        BindUi(ref _itemAttributeShowItemPanelRect, "AttributePanel/ItemAttributeShowItemPanel");
    }

    protected override void InitListener()
    {
        //变量绑定开始

        //变量绑定结束
        AddListenerEvent<Item, Vector3>("ShowItemAttribute", ShowItemAttribute);
        AddListenerEvent("HideItemAttribute", HideItemAttribute);
        AddListenerEvent<bool>("SetWindowDrag", SetWindowDrag);
    }

    //变量方法开始

    //变量方法结束

    //自定义属性开始

    //自定义属性结束
    private void SetWindowDrag(bool value)
    {
        _windowDrag = value;
    }

    private void ShowItemAttribute(Item item, Vector3 itemPos)
    {
        if (_windowDrag)
        {
            return;
        }

        _itemName.text = item.ItemName;
        _itemIcon.sprite = item.ItemIcon;
        _describe.text = item.Describe;
        for (int i = 0; i < _itemAttributeShowItems.Count; i++)
        {
            Destroy(_itemAttributeShowItems[i].gameObject);
        }

        _itemAttributeShowItems.Clear();

        for (int i = 0; i < item.attributeValueList.Count; i++)
        {
            GameObject itemAttributeShowItemObj = Instantiate(itemAttributeShowItemPrefab, _itemAttributeShowItemContent);
            ItemAttributeShowItem itemAttributeShowItem = itemAttributeShowItemObj.GetComponent<ItemAttributeShowItem>();
            itemAttributeShowItem.ViewStartInit();
            itemAttributeShowItem.InitAttribute(item.attributeValueList[i].attribute, item.attributeValueList[i].value);
            _itemAttributeShowItems.Add(itemAttributeShowItem);
        }

        _itemPos = itemPos;
        UpdateAttributePanelSize();
        _refreshLayout = true;
        DeleteTimeTask(_updateAttributePanelSizeTimeTask);
        _attributePanel.GetComponent<CanvasGroup>().alpha = 1;
        // _updateAttributePanelSizeTimeTask = AddTimeTask(() => { }, "显示布局", 0.02f);
    }

    private void UpdateAttributePanelSize()
    {
        //描述的位置
        _describeRect.anchoredPosition = new Vector2(0, -(itemIconSize + itemIconSSpacingDescribe));
        //描述的字数正好
        if (_describe.text.Length % 20 == 0)
        {
            _describeRect.sizeDelta = new Vector2(450, _describe.text.Length / 2f);
        }
        else
        {
            _describeRect.sizeDelta = new Vector2(450, _describe.text.Length / 2f + 1);
        }

        //描述的位置+描述的高+描述和属性标题的间距
        _attributeTitleRect.anchoredPosition = new Vector3(0, _describeRect.anchoredPosition.y - (_describeRect.sizeDelta.y + describeSpacingAttributeTitle));
        //属性标题的位置+属性标题的宽+属性标题和属性列表的间距
        _itemAttributeShowItemPanelRect.anchoredPosition = new Vector3(0, _attributeTitleRect.anchoredPosition.y - (60 + attributeTitleSpacingItemAttributeShowItemPanel));
        _itemAttributeShowItemPanelRect.sizeDelta = new Vector2(500, _itemAttributeShowItems.Count * 60);

        float size = itemIconSize + itemIconSSpacingDescribe + _describeRect.sizeDelta.y + describeSpacingAttributeTitle + 60 + attributeTitleSpacingItemAttributeShowItemPanel + _itemAttributeShowItemPanelRect.sizeDelta.y;
        _attributePanelRect.sizeDelta = new Vector2(500, size);
    }


    private void HideItemAttribute()
    {
        _attributePanel.GetComponent<CanvasGroup>().alpha = 0;
        _refreshLayout = false;
    }

    protected override void Update()
    {
        base.Update();
        if (_refreshLayout)
        {
            // UpdateAttributePanelSize();
            _attributePanel.transform.position = _itemPos /*+ offset*/;
        }
    }
}