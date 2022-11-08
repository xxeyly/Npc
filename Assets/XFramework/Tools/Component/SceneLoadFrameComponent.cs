using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    /// <summary>
    /// 场景组件--用于场景的加载
    /// </summary>
    public class SceneLoadFrameComponent : FrameComponent
    {
        public static SceneLoadFrameComponent Instance;
        private string _sceneName;
        private bool _asyncLoad;
        private AssetBundle _sceneAssetBundle;

        #region 异步加载场景

        private AsyncOperation _sceneAsyncOperation;

        public delegate void AsyncLoadSceneProgressDelegate(float progress, bool over);

        public AsyncLoadSceneProgressDelegate asyncLoadSceneProgress;

        #endregion

        public override void FrameInitComponent()
        {
            Instance = GetComponent<SceneLoadFrameComponent>();
        }
        public override void FrameSceneInitComponent()
        {
            
        }
        public override void FrameEndComponent()
        {
        }

        private void Update()
        {
            if (_asyncLoad)
            {
            }

            if (_sceneAsyncOperation != null)
            {
                if (_sceneAsyncOperation.progress < 0.9f)
                {
                    asyncLoadSceneProgress.Invoke(_sceneAsyncOperation.progress, _sceneAsyncOperation.isDone);
                }
                else
                {
                    asyncLoadSceneProgress.Invoke(_sceneAsyncOperation.progress, true);
                }
            }
        }

        public void SceneLoad(string sceneName)
        {
            _sceneName = sceneName;
        }

        /// <summary>
        /// 加载同步场景
        /// </summary>
        private void LoadSynchronizationScene(string sceneName)
        {
            Debug.Log("加载同步场景");
            if (!GameRootStart.Instance.dontDestroyOnLoad)
            {
                Destroy(GameRootStart.Instance.gameObject);
            }

            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.SceneComponentEnd();
            ViewFrameComponent.Instance.AllViewDestroy();
            TimeFrameComponent.Instance.FrameEndComponent();
            SceneManager.LoadScene(sceneName);
        }

        public void AsyncSceneIsDone()
        {
            if (_sceneAsyncOperation != null && _sceneAsyncOperation.progress >= 0.9f)
            {
                asyncLoadSceneProgress = null;
                _sceneAsyncOperation.allowSceneActivation = true;
                _sceneAsyncOperation = null;
            }
        }


        /// <summary>
        /// 退出程序
        /// </summary>
        public void SceneEsc()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.Quit();
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}