using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace XFramework
{
    public class BindUiType : MonoBehaviour
    {
        [LabelText("UI组件类型")] public General.UiType type;

        [LabelText("子级类型")] [ShowIf("@type == General.UiType.ChildList")]
        public MonoBehaviour childType;

        [ShowIf("@type == General.UiType.Button")] [LabelText("UI触发事件类型")]
        public General.UIEventTriggerType eventTriggerType;
    }
}