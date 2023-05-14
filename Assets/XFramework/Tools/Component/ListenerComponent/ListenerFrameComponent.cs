using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


namespace XFramework
{
    public partial class ListenerFrameComponent : FrameComponent
    {
        public static ListenerFrameComponent Instance;

        public delegate void CallBack();

        public delegate void CallBack<T>(T t);

        public delegate void CallBack<T, X>(T t, X x);

        public delegate void CallBack<T, X, Y>(T t, X x, Y y);

        public delegate void CallBack<T, X, Y, Z>(T t, X x, Y y, Z z);

        public delegate void CallBack<T, X, Y, Z, W>(T t, X x, Y y, Z z, W w);

        [LabelText("不带返回值的监听")] [SerializeField]
        private Dictionary<string, List<Delegate>> listenerCallBackDic = new Dictionary<string, List<Delegate>>();

        public delegate R ReturnCallBack<R>();

        public delegate R ReturnCallBack<T, R>(T t);

        public delegate R ReturnCallBack<T, X, R>(T t, X x);

        public delegate R ReturnCallBack<T, X, Y, R>(T t, X x, Y y);

        public delegate R ReturnCallBack<T, X, Y, Z, R>(T t, X x, Y y, Z z);

        public delegate R ReturnCallBack<T, X, Y, Z, W, R>(T t, X x, Y y, Z z, W w);

        [LabelText("带返回值的监听")] [SerializeField]
        private Dictionary<string, List<Delegate>> listenerReturnCallBackDic = new Dictionary<string, List<Delegate>>();

        // [LabelText("所有触发事件")] public List<string> allListener = new List<string>();
        [LabelText("所有触发事件")] public Dictionary<string, List<string>> allListener = new Dictionary<string, List<string>>();

        public override void FrameInitComponent()
        {
            Instance = GetComponent<ListenerFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }

        #region 不带返回值添加监听

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent(string eventType, CallBack callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void AddDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];
            if (!listenerCallBackDic.ContainsKey(eventClassName))
            {
                List<Delegate> delegates = new List<Delegate> { customDelegate };
                listenerCallBackDic.Add(eventClassName, delegates);
                if (!allListener.ContainsKey(eventClassName))
                {
                    allListener.Add(eventClassName, new List<string>() { });
                }

                if (allListener[eventClassName].Contains(eventName))
                {
                    Debug.Log("当前事件已经绑定过,即时清理");
                }
                else
                {
                    allListener[eventClassName].Add(eventName);
                }
            }
            else
            {
                List<Delegate> delegates = listenerCallBackDic[eventClassName];
                if (!delegates.Contains(customDelegate))
                {
                    delegates.Add(customDelegate);
                }
                else
                {
                    Debug.LogWarning(eventType + "该事件已经被绑定了");
                }
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T>(string eventType, CallBack<T> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X>(string eventType, CallBack<T, X> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y>(string eventType, CallBack<T, X, Y> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z>(string eventType, CallBack<T, X, Y, Z> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            CallBack<T, X, Y, Z, W> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        #endregion

        #region 带返回值添加监听

        /// <summary>
        /// 添加委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void AddReturnDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];
            if (!listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                List<Delegate> delegates = new List<Delegate> { customDelegate };
                listenerReturnCallBackDic.Add(eventClassName, delegates);
                if (!allListener.ContainsKey(eventClassName))
                {
                    allListener.Add(eventClassName, new List<string>() { });
                }

                if (allListener[eventClassName].Contains(eventName))
                {
                    Debug.Log(eventClassName + "_" + eventName + ":当前事件已经绑定过,即时清理");
                }
                else
                {
                    allListener[eventClassName].Add(eventName);
                }
            }
            else
            {
                List<Delegate> delegates = listenerReturnCallBackDic[eventClassName];
                if (!delegates.Contains(customDelegate))
                {
                    delegates.Add(customDelegate);
                }
                else
                {
                    Debug.LogWarning(eventType + "该事件已经被绑定了");
                }
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="R"></typeparam>
        public void AddReturnListenerEvent<R>(string eventType, ReturnCallBack<R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, R>(string eventType, ReturnCallBack<T, R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, R>(string eventType, ReturnCallBack<T, X, R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, R>(string eventType, ReturnCallBack<T, X, Y, R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType, ReturnCallBack<T, X, Y, Z, R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType,
            ReturnCallBack<T, X, Y, Z, W, R> callBack)
        {
            AddReturnDelegateToListenerEvent(eventType, callBack);
        }

        #endregion

        #region 移除监听

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent(string eventType)
        {
            RemoveDelegateToListenerEvent(eventType);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent(string eventType, CallBack callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T>(string eventType, CallBack<T> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, X>(string eventType, CallBack<T, X> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, X, Y>(string eventType, CallBack<T, X, Y> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, X, Y, Z>(string eventType, CallBack<T, X, Y, Z> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, X, Y, Z, W>(string eventType,
            CallBack<T, X, Y, Z, W> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        public void RemoveDelegateToListenerEvent(string eventType)
        {
            if (!listenerCallBackDic.ContainsKey(eventType) && !listenerReturnCallBackDic.ContainsKey(eventType))
            {
                Debug.Log(eventType + "没有被绑定过");
            }

            if (listenerCallBackDic.ContainsKey(eventType))
            {
                listenerCallBackDic.Remove(eventType);
            }

            if (listenerReturnCallBackDic.ContainsKey(eventType))
            {
                listenerReturnCallBackDic.Remove(eventType);
            }

            if (allListener.ContainsKey(eventType))
            {
                allListener.Remove(eventType);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void RemoveDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            if (!listenerCallBackDic.ContainsKey(eventType))
            {
                Debug.Log(eventType + "没有被绑定过");
            }
            else
            {
                List<Delegate> delegates = listenerCallBackDic[eventType];
                if (delegates.Contains(customDelegate))
                {
                    allListener.Remove(eventType);
                    delegates.Remove(customDelegate);
                }
                else
                {
                    Debug.Log(eventType + "没有被绑定过");
                }
            }
        }

        #endregion

        #region 执行无返回值监听

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        private void ExecuteEvent(string eventType)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];
            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack)customDelegate)();
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        private void ExecuteEvent<T>(string eventType, T t)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (t == null)
                    {
                        ((CallBack<T>)customDelegate)(t);
                        return;
                    }

                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack<T>)customDelegate)(t);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="x"></param>
        private void ExecuteEvent<T, X>(string eventType, T t, X x)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack<T, X>)customDelegate)(t, x);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y>(string eventType, T t, X x, Y y)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack<T, X, Y>)customDelegate)(t, x, y);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y, Z>(string eventType, T t, X x, Y y, Z z)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack<T, X, Y, Z>)customDelegate)(t, x, y, z);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y, Z, W>(string eventType, T t, X x, Y y, Z z, W w)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == w.GetType() && customDelegate.Method.Name == eventName)
                    {
                        ((CallBack<T, X, Y, Z, W>)customDelegate)(t, x, y, z, w);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        #endregion

        #region 执行有返回值监听

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        private R ExecuteReturnEvent<R>(string eventType)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<R>)customDelegate)();
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        private R ExecuteReturnEvent<T, R>(string eventType, T t)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<T, R>)customDelegate)(t);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="x"></param>
        private R ExecuteReturnEvent<T, X, R>(string eventType, T t, X x)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<T, X, R>)customDelegate)(t, x);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        private R ExecuteReturnEvent<T, X, Y, R>(string eventType, T t, X x, Y y)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<T, X, Y, R>)customDelegate)(t, x, y);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private R ExecuteReturnEvent<T, X, Y, Z, R>(string eventType, T t, X x, Y y, Z z)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<T, X, Y, Z, R>)customDelegate)(t, x, y, z);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private R ExecuteReturnEvent<T, X, Y, Z, W, R>(string eventType, T t, X x, Y y, Z z, W w)
        {
            string eventClassName = eventType.Split("-")[0];
            string eventName = eventType.Split("-")[1];

            if (listenerReturnCallBackDic.ContainsKey(eventClassName))
            {
                foreach (Delegate customDelegate in listenerReturnCallBackDic[eventClassName])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == w.GetType() && customDelegate.Method.Name == eventName)
                    {
                        return ((ReturnCallBack<T, X, Y, Z, W, R>)customDelegate)(t, x, y, z, w);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        #endregion
    }
}