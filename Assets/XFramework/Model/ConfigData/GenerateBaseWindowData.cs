using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class GenerateBaseWindowData
    {
        [LabelText("Using开始")] public static string startUsing = "引入开始";
        [LabelText("Using开始")] public static string endUsing = "引入结束";

        [LabelText("变量声明开始")] public static string startUiVariable = "变量声明开始";
        [LabelText("变量声明结束")] public static string endUiVariable = "变量声明结束";


        [LabelText("变量位置绑定开始")] public static string startVariableBindPath = "变量查找开始";
        [LabelText("变量位置绑定结束")] public static string endVariableBindPath = "变量查找结束";


        [LabelText("变量事件绑定开始")] public static string startVariableBindListener = "变量绑定开始";
        [LabelText("变量事件绑定结束")] public static string endVariableBindListener = "变量绑定结束";

        [LabelText("变量方法开始")] public static string startVariableBindEvent = "变量方法开始";
        [LabelText("变量方法结束")] public static string endVariableBindEvent = "变量方法结束";

        [LabelText("自定义属性开始")] public static string startCustomAttributesStart = "自定义属性开始";
        [LabelText("自定义属性结束")] public static string endCustomAttributesStart = "自定义属性结束";
    }
}