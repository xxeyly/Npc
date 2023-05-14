using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public partial class EntityFrameComponent : FrameComponent
    {
        public static EntityFrameComponent Instance;
        [Searchable] [LabelText("场景所有实体")] public List<EntityItem> sceneEntity;

        public delegate void DelegateOnShowEntity(string entityName);

        public DelegateOnShowEntity onShowEntity;

        public delegate void DelegateOnHideEntity(string entityName);

        public DelegateOnHideEntity onHideEntity;

        public GameObject Instantiate(GameObject instantiate)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate);
            InstantiateInit(tempInstantiate);
            return tempInstantiate;
        }

        public GameObject Instantiate(GameObject instantiate, Transform parent, bool world)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate, parent, world);
            InstantiateInit(tempInstantiate);

            return tempInstantiate;
        }

        private void InstantiateInit(GameObject instantiate)
        {
            foreach (EntityItem entityItem in instantiate.transform.GetComponentsInChildren<EntityItem>())
            {
                entityItem.AddToEntityList();
            }

            foreach (AnimatorControllerBase animatorControllerBase in instantiate.transform.GetComponentsInChildren<AnimatorControllerBase>())
            {
                animatorControllerBase.AddToAnimatorControllerList();
            }
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<EntityFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            EntityInit();
        }

        public override void FrameEndComponent()
        {
        }

        public void RemoveSceneEntityName()
        {
            List<EntityItem> tempEntityItems = DataFrameComponent.GetAllObjectsInScene<EntityItem>(GameRootStart.Instance.loadScene.name);
            for (int i = 0; i < tempEntityItems.Count; i++)
            {
                if (sceneEntity.Contains(tempEntityItems[i]))
                {
                    sceneEntity.Remove(tempEntityItems[i]);
                }
            }
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            List<EntityItem> tempEntity = new List<EntityItem>();
            //首场景,加载全部
            if (GameRootStart.Instance.loadScene.name == String.Empty)
            {
                tempEntity = DataFrameComponent.GetAllObjectsInScene<EntityItem>();
            }
            else
            {
                tempEntity = DataFrameComponent.GetAllObjectsInScene<EntityItem>(GameRootStart.Instance.loadScene.name);
            }

            foreach (EntityItem entityItem in tempEntity)
            {
                if (!sceneEntity.Contains(entityItem))
                {
                    sceneEntity.Add(entityItem);
                }
            }
        }

        public List<EntityItem> GetEntityItemByEntityName(string entityName)
        {
            List<EntityItem> entityItems = new List<EntityItem>();
            foreach (EntityItem tempSceEntityItem in sceneEntity)
            {
                if (tempSceEntityItem.entityName == entityName)
                {
                    entityItems.Add(tempSceEntityItem);
                }
            }

            return entityItems;
        }

        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        public void EntityAllHide()
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                entityItem.Hide();
            }
        }

        /// <summary>
        /// 实体全部显示
        /// </summary>
        public void EntityAllShow()
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                entityItem.Show();
            }
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFirstEntityItemByName<T>(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.GetComponent<T>();
                }
            }

            return default(T);
        }

        /// <summary>
        /// 根据实体名称获得第一个实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public EntityItem GetFirstEntityItemByName(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.GetComponent<EntityItem>();
                }
            }

            return null;
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    if (display)
                    {
                        entityItem.Show();
                    }
                    else
                    {
                        entityItem.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, params string[] entityNames)
        {
            foreach (string entityName in entityNames)
            {
                foreach (EntityItem entityItem in sceneEntity)
                {
                    if (entityName == entityItem.entityName)
                    {
                        if (display)
                        {
                            entityItem.Show();
                        }
                        else
                        {
                            entityItem.Hide();
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool GetFirstEntityStateByEntityName(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.gameObject.activeSelf;
                }
            }

            return false;
        }
    }
}