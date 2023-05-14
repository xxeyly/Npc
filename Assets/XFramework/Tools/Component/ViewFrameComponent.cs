using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework
{
    /// <summary>
    /// 视图组件
    /// </summary>
    public class ViewFrameComponent : FrameComponent
    {
        public static ViewFrameComponent Instance;

        [LabelText("视图类型与视图窗口的键值对")] [SerializeField]
        public Dictionary<Type, BaseWindow> activeViewDlc = new Dictionary<Type, BaseWindow>();

        /// <summary>
        /// 视图计时任务ID
        /// </summary>
        private int _viewTimeTaskId;

        /// <summary>
        /// 获得当前场景中的Canvas
        /// </summary>
        [HideInInspector] public Canvas canvas;

        /// <summary>
        /// 版本信息加载完毕任务
        /// </summary>
        private int _checkVersionInfoLoadOverTaskTime;

        public override void FrameInitComponent()
        {
            Instance = GetComponent<ViewFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            List<BaseWindow> tempSceneBaseWindow = DataFrameComponent.GetAllObjectsInScene<BaseWindow>(GameRootStart.Instance.loadScene.name);

            foreach (BaseWindow window in tempSceneBaseWindow)
            {
                if (!window.GetComponent<ChildBaseWindow>() && !activeViewDlc.ContainsKey(window.viewType))
                {
                    activeViewDlc.Add(window.viewType, window);
                    // Debug.Log(window.viewType + "初始化");
                    window.ViewStartInit();
                }
            }
            // ResetSetSiblingIndex();
        }

        public void RemoveView(Type viewType)
        {
            if (activeViewDlc.ContainsKey(viewType))
            {
                activeViewDlc.Remove(viewType);
            }
        }

        [Button("重置层级关系")]
        public void ResetSetSiblingIndex()
        {
            foreach (KeyValuePair<Type, BaseWindow> window in activeViewDlc)
            {
                window.Value.SetSetSiblingIndex();
            }
        }

        public override void FrameEndComponent()
        {
        }

        public GameObject Instantiate(GameObject instantiate, Transform parent, bool world)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate, parent, world);
            InstantiateInit(tempInstantiate);

            return tempInstantiate;
        }

        private void InstantiateInit(GameObject instantiate)
        {
            BaseWindow baseWindow = instantiate.GetComponent<BaseWindow>();
            if (!activeViewDlc.ContainsKey(baseWindow.viewType))
            {
                activeViewDlc.Add(baseWindow.viewType, baseWindow);
                baseWindow.ViewStartInit();
            }
        }

        /// <summary>
        /// 获得视图是否存在
        /// </summary>
        /// <returns></returns>
        private bool GetViewExistence(Type view)
        {
            if (activeViewDlc != null && activeViewDlc.ContainsKey(view))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool GetViewState(Type view)
        {
            if (activeViewDlc != null && activeViewDlc.ContainsKey(view))
            {
                return activeViewDlc[view].GetDisplay();
            }

            return false;
        }

        /// <summary>
        /// 所有视图初始化
        /// </summary>
        public void AllViewWindInit()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in activeViewDlc)
            {
                pair.Value.GetComponent<BaseWindow>().Init();
            }
        }

        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentActiveViewCount()
        {
            int currentActiveViewCount = 0;
            return activeViewDlc.Count - currentActiveViewCount;
        }

        /// <summary>
        /// 获得当前场景中视图的数量
        /// </summary>
        public int GetCurrentSceneViewCount()
        {
            return activeViewDlc.Count;
        }

        /// <summary>
        ///  删除所有视图
        /// </summary>
        public void DeleteAllView()
        {
            activeViewDlc.Clear();
        }

        #region 显示视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void ShowView(Type type)
        {
            activeViewDlc[type].DisPlay(true);
            activeViewDlc[type].Init();
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        public void ShowView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                ShowView(viewType);
            }
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(Type type, float time)
        {
            StopViewTimeTask();
            ViewTimeTask(ShowView, type, time);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(float time, params Type[] typeList)
        {
            StopViewTimeTask();
            ViewTimeTask(ShowView, typeList, time);
        }

        /// <summary>
        /// 消融View
        /// </summary>
        public void AblationView(Type viewType)
        {
            activeViewDlc[viewType].DisPlay(true);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void HideView(Type type)
        {
            BaseWindow baseWindow = activeViewDlc[type];
            if (GetViewState(type))
            {
                baseWindow.OnViewHide();
                baseWindow.DisPlay(false);
            }
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        public void HideView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                HideView(viewType);
            }
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void HideView(Type type, float time)
        {
            StopViewTimeTask();
            ViewTimeTask(HideView, type, time);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="types"></param>
        /// <param name="time">切换所需时间</param>
        public void HideView(float time, params Type[] types)
        {
            StopViewTimeTask();
            ViewTimeTask(HideView, types, time);
        }

        public void HideAllView()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in activeViewDlc)
            {
                if (pair.Value.GetViewShowType() == ViewShowType.Activity)
                {
                    HideView(pair.Key);
                }
            }
        }

        /// <summary>
        /// 冻结视图
        /// </summary>
        public void FrozenView(Type viewType)
        {
            activeViewDlc[viewType].DisPlay(false);
        }

        #endregion

        #region 视图任务

        /// <summary>
        /// 间隔一段时间后执行视图任务
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewAction">要执行的操作</param>
        /// <param name="viewType">视图类型</param>
        /// <param name="time">多长时间后执行</param>
        private void ViewTimeTask<T>(UnityAction<T> viewAction, T viewType, float time)
        {
            _viewTimeTaskId = TimeFrameComponent.Instance.AddTimeTask(() => { viewAction.Invoke(viewType); }, "视图任务", time);
        }

        /// <summary>
        /// 结束视图计时任务
        /// </summary>
        private void StopViewTimeTask()
        {
            TimeFrameComponent.Instance.DeleteTimeTask(_viewTimeTaskId);
        }

        #endregion

        /// <summary>
        /// 所有视图的隐藏摧毁任务
        /// </summary>
        public void AllViewDestroy(string destroySceneName)
        {
            List<BaseWindow> allWindows = DataFrameComponent.GetAllObjectsInScene<BaseWindow>(destroySceneName);
            foreach (BaseWindow window in allWindows)
            {
                //移除视图
                if (!window.GetComponent<ChildBaseWindow>() && activeViewDlc.ContainsKey(window.viewType))
                {
                    activeViewDlc.Remove(window.viewType);
                    window.OnViewDestroy();
                }
            }
        }
    }
}