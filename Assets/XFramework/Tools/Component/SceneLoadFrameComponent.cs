using System.Collections;
using System.Collections.Generic;
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
        public string sceneName;
        private bool _asyncLoad;
        private AssetBundle _sceneAssetBundle;

        #region 异步加载场景

        public delegate void AsyncLoadSceneProgressDelegate(float progress, bool over);

        public AsyncLoadSceneProgressDelegate asyncLoadSceneProgress;

        #endregion

        #region 增加场景

        public Dictionary<string, AsyncOperation> _addSceneAsyncOperation = new Dictionary<string, AsyncOperation>();

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

        public float GetAsyncSceneProgress(string sceneName)
        {
            if (_addSceneAsyncOperation.ContainsKey(sceneName))
            {
                if (_addSceneAsyncOperation[sceneName].isDone)
                {
                    return 1;
                }
                else
                {
                    return _addSceneAsyncOperation[sceneName].progress;
                }
            }

            return -1;
        }

        private void Update()
        {
            if (_addSceneAsyncOperation.Count > 0)
            {
                float allSceneLoadProgress = _addSceneAsyncOperation.Count * 0.9f;
                float currentSceneLoadProgress = 0;
                float sceneLoadProgress = 0;
                foreach (KeyValuePair<string, AsyncOperation> pair in _addSceneAsyncOperation)
                {
                    currentSceneLoadProgress += pair.Value.progress;
                }

                sceneLoadProgress = currentSceneLoadProgress / allSceneLoadProgress;
                if (sceneLoadProgress >= 1)
                {
                    asyncLoadSceneProgress?.Invoke(1, true);
                }
                else
                {
                    asyncLoadSceneProgress?.Invoke(1, false);
                }
            }
        }

        public void SceneLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            LoadSynchronizationScene(sceneName, loadSceneMode);
        }

        public void SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            LoadAsyncScene(sceneName, loadSceneMode);
        }

        private void LoadAsyncScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            AsyncOperation tempSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            tempSceneAsyncOperation.allowSceneActivation = false;
            if (!_addSceneAsyncOperation.ContainsKey(sceneName))
            {
                _addSceneAsyncOperation.Add(sceneName, tempSceneAsyncOperation);
            }
        }

        public void UnScene(string unSceneName)
        {
            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.SceneBeforeLoadPrepare(unSceneName);
            GameRootStart.Instance.SceneAssetBundleUnload();
            StartCoroutine(OnUnScene(unSceneName));
        }

        IEnumerator OnUnScene(string unSceneName)
        {
            AsyncOperation unSceneAsyncOperation = SceneManager.UnloadSceneAsync(unSceneName);
            yield return unSceneAsyncOperation;
        }

        public void UnSceneAndLoadScene(string unSceneName, string loadSceneName)
        {
            StartCoroutine(OnUnSceneAndLoadScene(unSceneName, loadSceneName));
        }

        IEnumerator OnUnSceneAndLoadScene(string unSceneName, string loadSceneName)
        {
            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.SceneBeforeLoadPrepare(unSceneName);
            GameRootStart.Instance.SceneAssetBundleUnload();
            AsyncOperation unSceneAsyncOperation = SceneManager.UnloadSceneAsync(unSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            yield return unSceneAsyncOperation;
            SceneLoad(loadSceneName, LoadSceneMode.Additive);
            AsyncSceneIsDone();
        }

        public void UnSceneAndLoadScenes(List<string> unSceneNames, List<string> loadSceneNames)
        {
            StartCoroutine(OnUnSceneAndLoadScenes(unSceneNames, loadSceneNames));
        }

        IEnumerator OnUnSceneAndLoadScenes(List<string> unSceneNames, List<string> loadSceneNames)
        {
            foreach (string unSceneName in unSceneNames)
            {
                //处理场景加载时需要卸载的逻辑
                GameRootStart.Instance.SceneBeforeLoadPrepare(unSceneName);
                GameRootStart.Instance.SceneAssetBundleUnload();
                AsyncOperation unSceneAsyncOperation = SceneManager.UnloadSceneAsync(unSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                yield return unSceneAsyncOperation;
            }

            foreach (string loadSceneName in loadSceneNames)
            {
                SceneLoad(loadSceneName, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// 加载同步场景
        /// </summary>
        private void LoadSynchronizationScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.SceneBeforeLoadPrepare(sceneName);
            GameRootStart.Instance.SceneAssetBundleUnload();
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        public void AsyncSceneIsDone()
        {
            asyncLoadSceneProgress = null;
            foreach (KeyValuePair<string, AsyncOperation> pair in _addSceneAsyncOperation)
            {
                pair.Value.allowSceneActivation = true;
            }

            _addSceneAsyncOperation.Clear();
            GameRootStart.Instance.SceneAssetBundleUnload();
        }

        public void StopAsyncLoad()
        {
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