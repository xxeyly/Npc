//引入开始
using UnityEngine.UI;
using TMPro;
//引入结束

using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[RequireComponent(typeof(ChildBaseWindowGenerateScripts))]
public class PurchaseItemsRequiredAttributes : ChildBaseWindow
{
    //变量声明开始
    private Image _attributesBack;
    private TextMeshProUGUI _attributesName;
    private Image _min;
    private Image _max;
    //变量声明结束

    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _attributesBack,"AttributesBack");
        BindUi(ref _attributesName,"AttributesName");
        BindUi(ref _min,"Min");
        BindUi(ref _max,"Max");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始

        //变量绑定结束
    }


    //变量方法开始

    //变量方法结束
}