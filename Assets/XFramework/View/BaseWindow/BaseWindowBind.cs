using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    partial class BaseWindow
    {
        #region UI 绑定

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">需要绑定的组件</param>
        /// <param name="path">当前组件的路径</param>
        // ReSharper disable once VirtualMemberCallInConstructor
        protected void BindUi<T>(ref T viewType, string path)
        {
            viewType = window.transform.Find(path).GetComponent<T>();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi<T>(ref List<T> viewType, string path)
        {
            viewType = new List<T>(window.transform.Find(path).GetComponentsInChildren<T>(true));
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">视图类型</param>
        /// <param name="path">路径</param>
        protected void BindUi(ref GameObject viewType, string path)
        {
            viewType = window.transform.Find(path).GetComponent<Transform>().gameObject;
        }

        #endregion

        #region UI 事件绑定

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="button"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(UnityEngine.UI.Button button, EventTriggerType eventId, UnityAction action)
        {
            button.onClick.AddListener(action);
            Debug.Log("tianjia1");
        }


        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(Selectable selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventId };
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(GameObject selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventId };
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="buttonList">当前要操作的UI组件</param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(List<Selectable> buttonList, EventTriggerType eventId,
            UnityAction<BaseEventData> action)
        {
            foreach (Selectable selectable in buttonList)
            {
                EventTrigger trigger = selectable.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = selectable.gameObject.AddComponent<EventTrigger>();
                }

                if (trigger.triggers.Count == 0)
                {
                    trigger.triggers = new List<EventTrigger.Entry>();
                }

                UnityAction<BaseEventData> callback = action;
                EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventId };
                entry.callback.AddListener(callback);
                trigger.triggers.Add(entry);
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="selectableList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<Selectable> SelectableConverter<T>(List<T> selectableList) where T : Selectable
        {
            List<Selectable> returnSelectableList = new List<Selectable>();
            foreach (T selectable in selectableList)
            {
                returnSelectableList.Add(selectable);
            }

            return returnSelectableList;
        }

        #endregion
    }
}