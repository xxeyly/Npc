#region 引入

using UnityEngine.UI;
using TMPro;

#endregion 引入

using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class PurchaseItemsRequiredAttributes : ChildBaseWindow
{
    #region 变量声明

    private Image _attributesBack;
    private TextMeshProUGUI _attributesName;
    private TMP_InputField _min;
    private TMP_InputField _max;

    #endregion 变量声明

    private ItemDemandItem _itemDemandItem;
    private AttributeValue _attributeValue;
    [LabelText("数据初始化")] [SerializeField] private bool dataInit;

    public override void Init()
    {
    }

    protected override void InitView()
    {
        #region 变量查找

        BindUi(ref _attributesBack, "AttributesBack");
        BindUi(ref _attributesName, "AttributesName");
        BindUi(ref _min, "Min");
        BindUi(ref _max, "Max");

        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定

        #endregion 变量绑定

        _min.onValueChanged.AddListener(OnMinValueChange);
        _max.onValueChanged.AddListener(OnMaxValueChange);
    }


    #region 变量方法

    #endregion 变量方法

    private void OnMinValueChange(string arg0)
    {
        if (!dataInit)
        {
            return;
        }

        ListenerFrameComponent.Instance.purchaseItems.OnEditorAttributeValue(_itemDemandItem, _attributeValue, new Vector2(int.Parse(_min.text), int.Parse(_max.text)));
    }

    private void OnMaxValueChange(string arg0)
    {
        if (!dataInit)
        {
            return;
        }

        ListenerFrameComponent.Instance.purchaseItems.OnEditorAttributeValue(_itemDemandItem, _attributeValue, new Vector2(int.Parse(_min.text), int.Parse(_max.text)));
    }

    public void SetAttributes(ItemDemandItem itemDemandItem, AttributeValue attributeValue, Vector2 value)
    {
        this._itemDemandItem = itemDemandItem;
        this._attributeValue = attributeValue;
        _attributesName.text = attributeValue.baseAttribute.AttributeName;
        _min.text = value.x.ToString();
        _max.text = value.y.ToString();
        dataInit = true;
    }
}