using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    [LabelText("场景质量")]
    public enum QualitySettingType
    {
        [LabelText("低")] Low,
        [LabelText("中")] Center,
        [LabelText("高")] High
    }

    [Serializable]
    public class TimeTaskList
    {
        public enum TimeLoopType
        {
            [LabelText("单程")] Once,
            [LabelText("循环")] Loop,
            [LabelText("不死")] Immortal,
        }

        [HideLabel] [HorizontalGroup("任务ID")] public int tid;
        [HideLabel] [HorizontalGroup("任务名称")] public string tidName;
        [HideLabel] [HorizontalGroup("结束时间")] public float endTime;
        [HideLabel] [HorizontalGroup("等待时间")] public float waitingTime;
        [HideLabel] [HorizontalGroup("任务类型")] public TimeLoopType loopType;
    }

    public static partial class General
    {
        #region 视图时间

        [LabelText("视图切换时间")] public const float ViewSwitchTime = 2f;

        [LabelText("视图错误时间")] public const float ViewErrorTime = 1f;

        #endregion

        [LabelText("Hierarchy内容跟随")] public static bool HierarchyContentFollow = true;

        [LabelText("获得网页跟目录地址")]
        public static string GetUrlRootPath()
        {
            string url = Application.absoluteURL;
            return url;
        }

        [LabelText("框架组件初始化顺序")] public static List<Type> frameComponentType = new List<Type>()
        {
            typeof(SceneLoadFrameComponent),
            typeof(RuntimeDataFrameComponent),
            typeof(EntityFrameComponent),
            typeof(ListenerFrameComponent),
            typeof(AudioFrameComponent),
            typeof(HttpFrameComponent),
            typeof(MouseFrameComponent),
            typeof(TimeFrameComponent),
            typeof(ViewFrameComponent),
            typeof(HotFixFrameComponent),
        };


        [LabelText("平台StreamingAssets路径")]
        public static string GetPlatformStreamingAssetsPath()
        {
#if UNITY_ANDROID //安卓  
            return Application.dataPath + "!/assets/";
#elif UNITY_IPHONE //iPhone  
            return Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR //windows平台和web平台  
            return Application.dataPath + "/StreamingAssets/";
#else
            return string.Empty;
#endif
        }

        [LabelText("平台PersistentDataPath路径")]
        public static string GetPlatformDownLoadDataPath()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return @"Assets\StreamingAssets\";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return Application.persistentDataPath + "/";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return Application.streamingAssetsPath + "/";
            }

            return string.Empty;
        }

        /// <summary>
        /// 获得文件数据地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileDataPath(string relativePath)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return GetUrlRootPath() + relativePath;
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + "/" + relativePath;
            }

            else if (Application.platform == RuntimePlatform.Android)
            {
                return "http://192.168.1.111/ZiYanGuDingYiChi/" + relativePath;
            }

            return "";
        }

        [LabelText("BaseWindow模板地址")] public static string BaseWindowTemplatePath =
            "Assets/XFramework/Model/Template/BaseWindowTemplate.cs";

        [LabelText("ChildBaseWindow模板地址")] public static string ChildBaseWindowTemplatePath =
            "Assets/XFramework/Model/Template/ChildBaseWindowTemplate.cs";

        [LabelText("CircuitBaseData模板地址")] public static string CircuitBaseDataTemplatePath =
            "Assets/XFramework/Model/Template/CircuitBaseDataTemplate.cs";

        [LabelText("ListenerComponentData模板地址")]
        public static string ListenerComponentDataTemplatePath =
            "Assets/XFramework/Model/Template/ListenerComponentDataTemplate.cs";

        [LabelText("SceneLoadComponent模板地址")] public static string SceneComponentTemplatePath =
            "Assets/XFramework/Model/Template/SceneComponentTemplate.cs";

        [LabelText("SceneLoadComponentInit模板地址")]
        public static string SceneComponentInitTemplatePath =
            "Assets/XFramework/Model/Template/SceneComponentInitTemplate.cs";

        [LabelText("AnimatorControllerParameterData模板地址")]
        public static string AnimatorControllerParameterDataTemplatePath =
            "Assets/XFramework/Model/Template/AnimatorControllerParameterDataTemplate.cs";

        [LabelText("存放路径根路径")] public static string assetRootPath =
            "Assets/Config/";

        [LabelText("存放路径根路径")] public static string assetScenePath =
            "Assets/Config/Scene/";

        [LabelText("自动打包配置存放路径")] public static string customBuildDataPath =
            assetRootPath + "CustomBuildData.asset";

        [LabelText("音频配置存放路径")] public static string customAudioDataPath =
            assetRootPath + "CustomAudioData.asset";

        [LabelText("框架配置存放路径")] public static string customFrameDataPath =
            assetRootPath + "CustomFrameData.asset";

        [LabelText("生成配置存放路径")] public static string generateBaseWindowPath =
            assetRootPath + "GenerateBaseWindowData.asset";

        [LabelText("场景配置存放路径")] public static string sceneLoadPath =
            assetRootPath + "SceneLoadData.asset";

        [LabelText("场景配置存放路径")] public static string buildSceneAssetBundleDataPath =
            assetRootPath + "BuildSceneAssetBundleData.asset";

        [LabelText("框架配置存放路径")] public static string frameComponentEditorDataPath =
            assetRootPath + "FrameComponentEditorData.asset";

        /// <summary>
        /// 生成属性类型
        /// </summary>
        public enum GenerateAttributesType
        {
            @int,
            @float,
            @string,
            @Sprite,
            @bool,
            @GameObject,
            @Transform,
            @Camera,
            @Color,
            @Texture,
            [LabelText("List<int>")] @List_int,
            [LabelText("List<float>")] @List_float,
            [LabelText("List<string>")] @List_string,
            [LabelText("List<Sprite>")] @List_Sprite,
            [LabelText("List<bool>")] @List_bool,
            [LabelText("List<GameObject>")] @List_GameObject,
            [LabelText("List<Transform>")] @List_Transform,
            [LabelText("List<Camera>")] @List_Camera,
            [LabelText("List<Color>")] @List_Color,
            [LabelText("List<Texture>")] @List_Texture,
        }

        /// <summary>
        /// 组件类型
        /// </summary>
        public enum UiType
        {
            Button,
            Image,
            Text,
            Toggle,
            RawImage,
            Scrollbar,
            Dropdown,
            InputField,
            ScrollRect,
            GameObject,
            Slider,
            VideoPlayer,

            // ReSharper disable once InconsistentNaming
            TextMeshProUGUI,

            // ReSharper disable once InconsistentNaming
            TMP_Dropdown,

            // ReSharper disable once InconsistentNaming
            TMP_InputField,
            ChildList,
        }

        /// <summary>
        /// 事件触发类型
        /// </summary>
        [System.Flags]
        public enum UIEventTriggerType
        {
            PointerClick = 1 << 1,
            PointerEnter = 1 << 2,
            PointerExit = 1 << 3,
            PointerDown = 1 << 4,
            PointerUp = 1 << 5,
            Drag = 1 << 6,
            Drop = 1 << 7,
            Scroll = 1 << 8,
            UpdateSelected = 1 << 9,
            Select = 1 << 10,
            Deselect = 1 << 11,
            Move = 1 << 12,
            InitializePotentialDrag = 1 << 13,
            BeginDrag = 1 << 14,
            EndDrag = 1 << 15,
            Submit = 1 << 16,
            Cancel = 1 << 17,

            All = PointerEnter | PointerExit | PointerDown | PointerUp | PointerClick | Drag | Drop | Scroll | UpdateSelected | Select | Deselect | Move | InitializePotentialDrag | BeginDrag | EndDrag | Submit | Cancel
        }

        /// <summary>
        /// BaseWindow属性
        /// </summary>
        [Serializable]
        public struct GenerateAttributesTypeGroup
        {
            [HorizontalGroup("属性类型")] [HideLabel] public GenerateAttributesType generateAttributesType;
            [HorizontalGroup("属性名称")] [HideLabel] public string attributesName;
            [HorizontalGroup("属性描述")] [HideLabel] public string attributesDescription;
        }
#if UNITY_EDITOR
        /// <summary>
        /// 平台类型
        /// </summary>
        public enum BuildTargetPlatform
        {
            StandaloneWindows64,
            WebGL,
            Android
        }

        [LabelText("场景加载方式")]
        public enum SceneLoadType
        {
            不加载,
            同步,
            异步,
        }
#endif
    }
}