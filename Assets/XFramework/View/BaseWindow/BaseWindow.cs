using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    [Serializable]
    public enum ViewShowType
    {
        [LabelText("活动")] Activity,
        [LabelText("静态")] Static
    }

    public enum ShowType
    {
        [LabelText("直接")] Direct,
        [LabelText("渐隐")] Curve
    }

    [Serializable]
    public struct TimeTaskInfo
    {
        [HideLabel] [HorizontalGroup("任务ID")] public int timeTaskId;
        [HideLabel] [HorizontalGroup("任务名称")] public string timeTaskName;
        [HideLabel] [HorizontalGroup("任务类型")] public TimeTaskList.TimeLoopType timeLoopType;
    }

    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract partial class BaseWindow : SerializedMonoBehaviour
    {
        protected GameObject window;
        protected CanvasGroup canvasGroup;

        [HorizontalGroup("标签")] [BoxGroup("标签/属性")] [LabelText("视图类型")] [SerializeField] [EnumToggleButtons] [LabelWidth(50)]
        protected ViewShowType viewShowType = ViewShowType.Activity;

        [BoxGroup("标签/属性")] [LabelText("显示类型")] [SerializeField] [EnumToggleButtons] [LabelWidth(50)]
        protected ShowType showType = ShowType.Direct;

        [BoxGroup("标签/属性")] [LabelText("显示时间")] [Range(0.1f, 9)] [SerializeField] /*[ShowIf("showType", ShowType.Curve)]*/[EnableIf("showType", ShowType.Curve)]
        protected float showTime = 1;

        [BoxGroup("标签/属性")] [LabelText("UI层级")] [SerializeField] [EnumToggleButtons] [LabelWidth(50)]
        public int layerIndex = 0;

        [BoxGroup("调试")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("日志输出")]
        public bool isLog;

        [BoxGroup("调试")] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [Searchable] [SerializeField] [LabelText("计时任务列表")]
        protected List<TimeTaskInfo> timeTaskInfoList = new List<TimeTaskInfo>();

        public Type viewType;

        [BoxGroup("标签/命名")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

        [BoxGroup("标签/命名")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("类名称")] [LabelWidth(50)]
        public string typeName;
        [BoxGroup("标签/命名")]
        [Button(ButtonSizes.Medium)]
        [LabelText("重命名")]
        [GUIColor(0, 1, 0)]
        public void GameNameSet()
        {
            gameObject.name = viewType.Name;
            typeName = viewType.Name;
        }

        /// <summary>
        /// 视图显示任务
        /// </summary>
        private int _viewShowTimeTask;

        protected BaseWindow()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            viewType = InitViewType();
            // ReSharper disable once VirtualMemberCallInConstructor
            InitViewShowType();
        }

        /// <summary>
        /// 返回当前视图的活动类型
        /// </summary>
        /// <returns></returns>
        public ViewShowType GetViewShowType()
        {
            return viewShowType;
        }

        

        /// <summary>
        /// 视图初始化
        /// </summary>
        public virtual void ViewStartInit()
        {
            window = transform.Find("Window").gameObject;
            canvasGroup = window.GetComponent<CanvasGroup>();
            InitView();
            InitListener();
            OnlyOnceInit();
        }

        public void SetSetSiblingIndex()
        {
            transform.SetSiblingIndex(layerIndex);
        }


        public abstract void Init();

        /// <summary>
        /// 初始化视图的显示类型
        /// </summary>
        protected virtual void InitViewShowType()
        {
        }

        /// <summary>
        /// UI类型设置
        /// </summary>
        protected Type InitViewType()
        {
            return GetType();
        }

        /// <summary>
        /// UI绑定
        /// </summary>
        protected abstract void InitView();

        /// <summary>
        /// 事件监听
        /// </summary>
        protected abstract void InitListener();

        /// <summary>
        /// 仅仅初始化一次
        /// </summary>
        protected virtual void OnlyOnceInit()
        {
        }

        /// <summary>
        /// 视图摧毁
        /// </summary>
        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideThisView()
        {
            ViewFrameComponent.Instance.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            ViewFrameComponent.Instance.ShowView(viewType);
        }

       

        /// <summary>
        /// 获得当前视图的显示状态
        /// </summary>
        /// <returns></returns>
        public bool GetDisplay()
        {
            return window.activeSelf;
        }
    }
}