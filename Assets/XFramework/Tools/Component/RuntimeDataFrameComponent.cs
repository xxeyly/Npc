using Sirenix.OdinInspector;

namespace XFramework
{
    /// <summary>
    /// 动态加载数据
    /// </summary>
    public partial class RuntimeDataFrameComponent : FrameComponent
    {
        public static RuntimeDataFrameComponent Instance;
       
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