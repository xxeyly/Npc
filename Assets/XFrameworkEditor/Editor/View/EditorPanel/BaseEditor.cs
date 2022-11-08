#if UNITY_EDITOR

namespace XFramework
{
    public abstract class BaseEditor
    {
        public abstract void OnDisable();
        public abstract void OnCreateConfig();
        public abstract void OnSaveConfig();

        public abstract void OnLoadConfig();

        public abstract void OnInit();
    }
}
#endif
