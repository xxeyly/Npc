using Sirenix.OdinInspector;

namespace XFramework
{
    /// <summary>
    /// 动态加载数据
    /// </summary>
    public partial class RuntimeDataFrameComponent : FrameComponent
    {
        public static RuntimeDataFrameComponent Instance;
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool jump;

        [BoxGroup("场景加载")] [LabelText("跳转场景名称")]
        public string jumpSceneName;

        [LabelText("音乐开关")] public bool audioState;
        [LabelText("当前质量")] public QualitySettingType qualitySettingType = QualitySettingType.High;
        [LabelText("鼠标状态")] public bool mouseState;
        public override void FrameEndComponent()
        {
        }
        public override void FrameSceneInitComponent()
        {
            
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<RuntimeDataFrameComponent>();
        }
    }
}