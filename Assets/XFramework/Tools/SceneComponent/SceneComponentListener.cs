using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework
{
    public partial class SceneComponent
    {
        [SerializeField] [LabelText("当前监听")] private List<string> callBackName = new List<string>();

        #region 增加事件

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void AddListenerEvent(string eventType, ListenerFrameComponent.CallBack unityAction)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, unityAction);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T>(string eventType, ListenerFrameComponent.CallBack<T> callBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X>(string eventType, ListenerFrameComponent.CallBack<T, X> callBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y>(string eventType, ListenerFrameComponent.CallBack<T, X, Y> callBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z>(string eventType,
            ListenerFrameComponent.CallBack<T, X, Y, Z> callBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerFrameComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddListenerEvent(GetType() + "-" + eventType, callBack);
        }

        #endregion

        #region 删除事件

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void RemoveListenerEvent(string eventType, ListenerFrameComponent.CallBack unityAction)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, unityAction);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T>(string eventType, ListenerFrameComponent.CallBack<T> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X>(string eventType, ListenerFrameComponent.CallBack<T, X> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y>(string eventType, ListenerFrameComponent.CallBack<T, X, Y> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z>(string eventType,
            ListenerFrameComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerFrameComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(GetType() + "-" + eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        private void RemoveListenerEvent(string eventType)
        {
            ListenerFrameComponent.Instance.RemoveDelegateToListenerEvent(eventType);
        }

        public void RemoveAllListenerEvent()
        {
            //有事件监听的才移除
            if (callBackName.Count > 0)
            {
                callBackName.Clear();
                RemoveListenerEvent(GetType().ToString());
            }
        }

        #endregion

        #region 返回方法

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<R>(string eventType, ListenerFrameComponent.ReturnCallBack<R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, R>(string eventType,
            ListenerFrameComponent.ReturnCallBack<T, R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, R>(string eventType,
            ListenerFrameComponent.ReturnCallBack<T, X, R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, R>(string eventType,
            ListenerFrameComponent.ReturnCallBack<T, X, Y, R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType,
            ListenerFrameComponent.ReturnCallBack<T, X, Y, Z, R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType,
            ListenerFrameComponent.ReturnCallBack<T, X, Y, Z, W, R> returnCallBack)
        {
            callBackName.Add(GetType() + "-" + eventType);

            ListenerFrameComponent.Instance.AddReturnListenerEvent(GetType() + "-" + eventType, returnCallBack);
        }

        #endregion
    }
}