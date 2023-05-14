using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class GenerateBaseWindowData : ScriptableObject
    {
        [LabelText("Using开始")] public string startUsing = "#region 引入";
        [LabelText("Using开始")] public string endUsing = "#endregion 引入";

        [LabelText("变量声明")] public string startUiVariable = "#region 变量声明";
        [LabelText("变量声明")] public string endUiVariable = "#endregion 变量声明";


        [LabelText("变量位置绑定开始")] public string startVariableBindPath = "#region 变量查找";
        [LabelText("变量位置绑定结束")] public string endVariableBindPath = "#endregion 变量查找";


        [LabelText("变量事件绑定开始")] public string startVariableBindListener = "#region 变量绑定";
        [LabelText("变量事件绑定结束")] public string endVariableBindListener = "#endregion 变量绑定";

        [LabelText("变量方法")] public string startVariableBindEvent = "#region 变量方法";
        [LabelText("变量方法")] public string endVariableBindEvent = "#endregion 变量方法";

        [LabelText("自定义属性开始")] public string startCustomAttributesStart = "#region 自定义属性";
        [LabelText("自定义属性结束")] public string endCustomAttributesStart = "#endregion 自定义属性";
        
    }
}