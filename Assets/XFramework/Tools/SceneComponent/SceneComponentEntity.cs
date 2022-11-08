using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public partial class SceneComponent
    {
        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        protected void DisplayEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            EntityFrameComponent.Instance.DisplayEntityGroup(groupTag, display, hideOther);
        }

        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        protected void DisplayEntityGroup(bool display, params string[] groupTag)
        {
            EntityFrameComponent.Instance.DisplayEntityGroup(display, groupTag);
        }

        protected List<EntityItem> GetEntityItemByEntityGroupName(string groupName)
        {
            return EntityFrameComponent.Instance.GetEntityItemByEntityGroupName(groupName);
        }

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

        /// <summary>
        /// 根据名字或者所有实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        protected List<EntityItem> GetEntityItemByEntityName(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntityItemByEntityName(entityName);
        }
    }
}