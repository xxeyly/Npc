using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    [Serializable]
    public class EntityComponentDataInfo
    {
        [HideLabel] [HorizontalGroup("实体标签")] public string entityGroupTag;
        [HideLabel] [HorizontalGroup("实体组")] public List<EntityItem> entityGroup;
    }

    [Serializable]
    public class EditorEntityComponentDataInfo
    {
        [HideLabel] [HorizontalGroup("实体标签")] public string entityGroupTag;
        [HideLabel] [HorizontalGroup("实体组")] public List<EntityItem> entityGroup;

        [HideLabel]
        [Button("仅显示当前组")]
        public void OnOnlyShow()
        {
            EntityFrameComponent entityFrameComponent = UnityEngine.Object.FindObjectOfType<EntityFrameComponent>();
            entityFrameComponent.DisplayEditorEntityGroup(entityGroupTag, true, true);
        }
    }

    public partial class EntityFrameComponent : FrameComponent
    {
        public static EntityFrameComponent Instance;
        [Searchable] [LabelText("场景所有实体")] public List<EntityItem> sceneEntity;

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("实体组")]
        public List<EntityComponentDataInfo> entityComponentDataInfos;

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("编辑器实体组")]
        public List<EditorEntityComponentDataInfo> editorEntityComponentDataInfos;

        public delegate void DelegateOnShowEntity(string entityName);

        public DelegateOnShowEntity onShowEntity;

        public delegate void DelegateOnHideEntity(string entityName);

        public DelegateOnHideEntity onHideEntity;

        public void TryAddEntity(EntityItem entityItem)
        {
            //读取添加的每个实体的所有实体组
            for (int i = 0; i < entityItem.entityTags.Count; i++)
            {
                bool isCon = false;
                foreach (EntityComponentDataInfo entityComponentDataInfo in entityComponentDataInfos)
                {
                    if (entityComponentDataInfo.entityGroupTag == entityItem.entityTags[i])
                    {
                        isCon = true;
                        entityComponentDataInfo.entityGroup.Add(entityItem);
                    }
                }

                if (!isCon)
                {
                    entityComponentDataInfos.Add(new EntityComponentDataInfo()
                    {
                        entityGroupTag = entityItem.entityTags[i],
                        entityGroup = new List<EntityItem>() { entityItem }
                    });
                }
            }
#if UNITY_EDITOR
            for (int i = 0; i < entityItem.editorEntityTags.Count; i++)
            {
                bool isCon = false;
                foreach (EditorEntityComponentDataInfo editorEntityComponentDataInfo in editorEntityComponentDataInfos)
                {
                    if (editorEntityComponentDataInfo.entityGroupTag == entityItem.editorEntityTags[i])
                    {
                        isCon = true;
                        editorEntityComponentDataInfo.entityGroup.Add(entityItem);
                    }
                }

                if (!isCon)
                {
                    editorEntityComponentDataInfos.Add(new EditorEntityComponentDataInfo()
                    {
                        entityGroupTag = entityItem.editorEntityTags[i],
                        entityGroup = new List<EntityItem>() { entityItem }
                    });
                }
            }
#endif
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<EntityFrameComponent>();
            EntityInit();

        }
        public override void FrameSceneInitComponent()
        {
            
        }

        public override void FrameEndComponent()
        {
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            entityComponentDataInfos.Clear();
#if UNITY_EDITOR
            editorEntityComponentDataInfos.Clear();
#endif
            // sceneEntity = new List<EntityItem>(GameObject.FindObjectsOfType<EntityItem>());
            sceneEntity = DataComponent.GetAllObjectsInScene<EntityItem>();
            foreach (EntityItem entityItem in sceneEntity)
            {
                TryAddEntity(entityItem);
            }
        }


        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="display"></param>
        /// <param name="groupTag"></param>
        public void DisplayEntityGroup(bool display, params string[] groupTag)
        {
            foreach (string groupName in groupTag)
            {
                DisplayEntityGroup(groupName, display, false);
            }
        }

        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            List<EntityItem> entityGroup = null;
            foreach (EntityComponentDataInfo entityComponentDataInfo in entityComponentDataInfos)
            {
                if (entityComponentDataInfo.entityGroupTag == groupTag)
                {
                    entityGroup = entityComponentDataInfo.entityGroup;
                    break;
                }
            }

            if (entityGroup == null)
            {
                Debug.LogError("实体组未定义");
                return;
            }

            //先显示
            foreach (EntityItem entityItem in entityGroup)
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

            if (hideOther)
            {
                //创建一个临时场景实体组
                List<EntityItem> tempSceEntityItems = new List<EntityItem>();
                foreach (EntityItem entityItem in sceneEntity)
                {
                    tempSceEntityItems.Add(entityItem);
                }

                //移除当前要显示的实体
                foreach (EntityItem entityItem in entityGroup)
                {
                    tempSceEntityItems.Remove(entityItem);
                }

                //隐藏剩余实体
                foreach (EntityItem tempSceEntityItem in tempSceEntityItems)
                {
                    tempSceEntityItem.Hide();
                }

                //清空
                tempSceEntityItems.Clear();
            }
        }

        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEditorEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            List<EntityItem> entityGroup = null;
            foreach (EditorEntityComponentDataInfo editorEntityComponentDataInfo in editorEntityComponentDataInfos)
            {
                if (editorEntityComponentDataInfo.entityGroupTag == groupTag)
                {
                    entityGroup = editorEntityComponentDataInfo.entityGroup;
                    break;
                }
            }

            if (entityGroup == null)
            {
                Debug.LogError("实体组未定义");
                return;
            }

            //先显示
            foreach (EntityItem entityItem in entityGroup)
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

            if (hideOther)
            {
                //创建一个临时场景实体组
                List<EntityItem> tempSceEntityItems = new List<EntityItem>();
                foreach (EntityItem entityItem in sceneEntity)
                {
                    tempSceEntityItems.Add(entityItem);
                }

                //移除当前要显示的实体
                foreach (EntityItem entityItem in entityGroup)
                {
                    tempSceEntityItems.Remove(entityItem);
                }

                //隐藏剩余实体
                foreach (EntityItem tempSceEntityItem in tempSceEntityItems)
                {
                    tempSceEntityItem.Hide();
                }

                //清空
                tempSceEntityItems.Clear();
            }
        }

        public List<EntityItem> GetEntityItemByEntityGroupName(string groupName)
        {
            foreach (EntityComponentDataInfo entityComponentDataInfo in entityComponentDataInfos)
            {
                if (entityComponentDataInfo.entityGroupTag == groupName)
                {
                    return entityComponentDataInfo.entityGroup;
                }
            }

            return null;
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
            foreach (EntityComponentDataInfo entityComponentDataInfo in entityComponentDataInfos)
            {
                //先显示
                foreach (EntityItem entityItem in entityComponentDataInfo.entityGroup)
                {
                    entityItem.Show();
                }
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