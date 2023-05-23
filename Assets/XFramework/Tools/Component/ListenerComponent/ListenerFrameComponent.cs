using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;


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

        public delegate R ReturnCallBack<R>();

        public delegate R ReturnCallBack<T, R>(T t);

        public delegate R ReturnCallBack<T, X, R>(T t, X x);

        public delegate R ReturnCallBack<T, X, Y, R>(T t, X x, Y y);

        public delegate R ReturnCallBack<T, X, Y, Z, R>(T t, X x, Y y, Z z);

        public delegate R ReturnCallBack<T, X, Y, Z, W, R>(T t, X x, Y y, Z z, W w);

        [LabelText("所有触发事件")] public Dictionary<string, List<Delegate>> allListener = new Dictionary<string, List<Delegate>>();

        public override void FrameInitComponent()
        {
            Instance = GetComponent<ListenerFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            OnGetAllAddListenerEvent();
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
            if (!allListener.ContainsKey(eventType))
            {
                allListener.Add(eventType, new List<Delegate>());
            }

            List<Delegate> currentDelegateList = allListener[eventType];
            if (!currentDelegateList.Contains(customDelegate))
            {
                currentDelegateList.Add(customDelegate);
            }
            else
            {
                Debug.LogWarning(eventType + "该事件已经被绑定了");
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
        public void AddListenerEvent<T, X, Y, Z, W>(string eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        #endregion

        #region 带返回值添加监听

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="R"></typeparam>
        public void AddReturnListenerEvent<R>(string eventType, ReturnCallBack<R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, R>(string eventType, ReturnCallBack<T, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, R>(string eventType, ReturnCallBack<T, X, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, R>(string eventType, ReturnCallBack<T, X, Y, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType, ReturnCallBack<T, X, Y, Z, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType, ReturnCallBack<T, X, Y, Z, W, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
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
            if (allListener.ContainsKey(eventType))
            {
                allListener.Remove(eventType);
            }
            else
            {
                Debug.Log(eventType + "没有被绑定过");
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void RemoveDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            if (!allListener.ContainsKey(eventType))
            {
                Debug.Log(eventType + "没有被绑定过");
            }
            else
            {
                List<Delegate> delegates = allListener[eventType];
                if (delegates.Contains(customDelegate))
                {
                    delegates.Remove(customDelegate);
                }
                else
                {
                    Debug.Log(eventType + "没有被绑定过");
                }

                if (delegates.Count == 0)
                {
                    allListener.Remove(eventType);
                }
            }
        }

        #endregion

        #region 执行无返回值监听

        private void ExecuteEvent(string eventType, string delegateType)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 && customDelegate.Method.Name == delegateType)
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

        private void ExecuteEvent<T>(string eventType, string delegateType, T t)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (t == null)
                    {
                        ((CallBack<T>)customDelegate)(t);
                        return;
                    }

                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private void ExecuteEvent<T, X>(string eventType, string delegateType, T t, X x)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private void ExecuteEvent<T, X, Y>(string eventType, string delegateType, T t, X x, Y y)
        {
            if (allListener.ContainsKey(eventType))

            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private void ExecuteEvent<T, X, Y, Z>(string eventType, string delegateType, T t, X x, Y y, Z z)
        {
            if (allListener.ContainsKey(eventType))

            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private void ExecuteEvent<T, X, Y, Z, W>(string eventType, string delegateType, T t, X x, Y y, Z z, W w)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == w.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<R>(string eventType, string delegateType)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<T, R>(string eventType, string delegateType, T t)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<T, X, R>(string eventType, string delegateType, T t, X x)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<T, X, Y, R>(string eventType, string delegateType, T t, X x, Y y)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<T, X, Y, Z, R>(string eventType, string delegateType, T t, X x, Y y, Z z)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        private R ExecuteReturnEvent<T, X, Y, Z, W, R>(string eventType, string delegateType, T t, X x, Y y, Z z, W w)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == w.GetType() &&
                        customDelegate.Method.Name == delegateType)
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

        public void OnGetAllAddListenerEvent()
        {
            foreach (SceneComponent sceneComponent in DataFrameComponent.GetAllObjectsInScene<SceneComponent>())
            {
                ReflexBinEventListener(sceneComponent);
            }

            foreach (BaseWindow baseWindow in DataFrameComponent.GetAllObjectsInScene<BaseWindow>())
            {
                if (baseWindow.GetComponent<ChildBaseWindow>())
                {
                    continue;
                }

                ReflexBinEventListener(baseWindow);
            }
        }

        private void ReflexBinEventListener(Object targetObject)
        {
            //反射所有Type中的方法
            foreach (MethodInfo methodInfo in targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                //获得该方法所有使用的特性
                foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
                {
                    //当前特性是监听事件
                    if (customAttribute is AddListenerEventAttribute)
                    {
                        Type[] parameterInfo;
                        if (methodInfo.ReturnType == typeof(void))
                        {
                            parameterInfo = new Type[methodInfo.GetParameters().Length];
                            for (int i = 0; i < methodInfo.GetParameters().Length; i++)
                            {
                                parameterInfo[i] = methodInfo.GetParameters()[i].ParameterType;
                            }
                        }
                        else
                        {
                            parameterInfo = new Type[methodInfo.GetParameters().Length + 1];
                            for (int i = 0; i < methodInfo.GetParameters().Length; i++)
                            {
                                parameterInfo[i] = methodInfo.GetParameters()[i].ParameterType;
                            }

                            parameterInfo[methodInfo.GetParameters().Length] = methodInfo.ReturnType;
                        }


                        Type delegateType = null;
                        if (methodInfo.ReturnType != typeof(void))
                        {
                            if (parameterInfo.Length == 1)
                            {
                                delegateType = typeof(ReturnCallBack<>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 2)
                            {
                                delegateType = typeof(ReturnCallBack<,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 3)
                            {
                                delegateType = typeof(ReturnCallBack<,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 4)
                            {
                                delegateType = typeof(ReturnCallBack<,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 5)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 6)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,>).MakeGenericType(parameterInfo);
                            }
                        }
                        else
                        {
                            if (methodInfo.GetParameters().Length == 0)
                            {
                                delegateType = typeof(CallBack);
                            }
                            else
                            {
                                if (parameterInfo.Length == 1)
                                {
                                    delegateType = typeof(CallBack<>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 2)
                                {
                                    delegateType = typeof(CallBack<,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 3)
                                {
                                    delegateType = typeof(CallBack<,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 4)
                                {
                                    delegateType = typeof(CallBack<,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 5)
                                {
                                    delegateType = typeof(CallBack<,,,,>).MakeGenericType(parameterInfo);
                                }
                            }
                        }

                        if (delegateType != null)
                        {
                            Delegate tempDelegate = Delegate.CreateDelegate(delegateType, targetObject, methodInfo);
                            AddDelegateToListenerEvent(targetObject.GetType().Name, tempDelegate);
                        }
                    }
                }
            }
        }
    }
}