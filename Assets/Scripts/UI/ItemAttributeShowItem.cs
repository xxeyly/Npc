//引入开始

using UnityEngine.UI;
//引入结束
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class ItemAttributeShowItem : ChildBaseWindow
{
    //变量声明开始
    private Text _content;
    //变量声明结束

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _content, "Content");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始

        //变量绑定结束
    }


    //变量方法开始

    //变量方法结束
    public void InitAttribute(Attribute attribute, int attributeValue)
    {
        _content.text = attribute.AttributeName + ":" + attributeValue;
    }
}