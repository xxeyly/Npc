using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        protected void EntityAllHide()
        {
            EntityFrameComponent.Instance.EntityAllHide();
        }

        /// <summary>
        /// 实体全部显示
        /// </summary>
        protected void EntityAllShow()
        {
            EntityFrameComponent.Instance.EntityAllShow();
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetFirstEntityItemByName<T>(string entityName)
        {
            return EntityFrameComponent.Instance.GetFirstEntityItemByName<T>(entityName);
        }

        public EntityItem GetFirstEntityItemByName(string entityName)
        {
            return EntityFrameComponent.Instance.GetFirstEntityItemByName(entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        protected void DisplayEntityByEntityName(bool display, string entityName)
        {
            EntityFrameComponent.Instance.DisplayEntityByEntityName(display, entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        protected void DisplayEntityByEntityName(bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntityByEntityName(display, entityNames);
        }

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        protected bool GetFirstEntityStateByEntityName(string entityName)
        {
            return EntityFrameComponent.Instance.GetFirstEntityStateByEntityName(entityName);
        }
    }
}