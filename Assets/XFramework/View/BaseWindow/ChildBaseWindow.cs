using Sirenix.OdinInspector;

namespace XFramework
{
    /// <summary>
    /// 局部UI界面
    /// </summary>
    public abstract class ChildBaseWindow : BaseWindow
    {
        [LabelText("索引")] public int itemIndex;

        public override void ViewStartInit()
        {
            window = gameObject;
            InitView();
            InitListener();
            OnlyOnceInit();
        }

        /// <summary>
        /// 选中
        /// </summary>
        public virtual void OnSelect()
        {
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        public virtual void OnUnSelect()
        {
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="itemIndex"></param>
        public virtual void InitData(int itemIndex)
        {
            this.itemIndex = itemIndex;
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="content"></param>
        public virtual void InitData(int itemIndex, string content)
        {
            this.itemIndex = itemIndex;
            
        }
    }
}