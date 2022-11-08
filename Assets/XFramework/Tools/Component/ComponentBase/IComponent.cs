namespace XFramework
{
    public interface IComponent
    {
        /// <summary>
        /// 框架初始化
        /// </summary>
        void FrameInitComponent();

        /// <summary>
        /// 框架场景初始化
        /// </summary>
        void FrameSceneInitComponent();
    }
}