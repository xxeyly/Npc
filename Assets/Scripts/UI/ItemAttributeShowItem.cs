#region 引入

using UnityEngine.UI;
#endregion 引入
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemAttributeShowItem : ChildBaseWindow
{
    #region 变量声明
    private Text _content;
    #endregion 变量声明

    public override void Init()
    {
    }

    protected override void InitView()
    {
       #region 变量查找
        BindUi(ref _content, "Content");
        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定

        #endregion 变量绑定
    }


    #region 变量方法

    #endregion 变量方法
    public void InitAttribute(BaseAttribute baseAttribute, int attributeValue)
    {
        _content.text = baseAttribute.AttributeName + ":" + attributeValue;
    }
}