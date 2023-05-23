using System;
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        protected bool GetViewState(Type view)
        {
            return ViewFrameComponent.Instance.GetViewState(view);
        }


        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentActiveViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentActiveViewCount();
        }


        #region 显示视图

        protected void ShowThisView()
        {
            ViewFrameComponent.Instance.ShowView(viewType);
        }

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void ShowView(Type type)
        {
            ViewFrameComponent.Instance.ShowView(type);
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void ShowView(params Type[] types)
        {
            ViewFrameComponent.Instance.ShowView(types);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        protected void ShowView(Type type, float time)
        {
            ViewFrameComponent.Instance.ShowView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="time">切换所需时间</param>
        protected void ShowView(float time, params Type[] typeList)
        {
            ViewFrameComponent.Instance.ShowView(time, typeList);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void HideView(Type type)
        {
            ViewFrameComponent.Instance.HideView(type);
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void HideView(params Type[] types)
        {
            ViewFrameComponent.Instance.HideView(types);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        protected void HideView(Type type, float time)
        {
            ViewFrameComponent.Instance.HideView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="types"></param>
        /// <param name="time">切换所需时间</param>
        protected void HideView(float time, params Type[] types)
        {
            ViewFrameComponent.Instance.HideView(time, types);
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideAllView()
        {
            ViewFrameComponent.Instance.HideAllView();
        }

        #endregion

        public virtual void OnViewHide()
        {
        }

        /// <summary>
        /// 界面摧毁
        /// </summary>
        public virtual void OnViewDestroy()
        {
            RemoveThisView();
            RemoveTimeTask();
        }

        private void RemoveThisView()
        {
            // Debug.Log(viewType + "摧毁");
            ViewFrameComponent.Instance.RemoveView(GetType());
        }
    }
}