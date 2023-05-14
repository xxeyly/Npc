using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneMode"></param>
        protected void SceneLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneLoadFrameComponent.Instance.SceneLoad(sceneName, loadSceneMode);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneMode"></param>
        protected void SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneLoadFrameComponent.Instance.SceneAsyncLoad(sceneName, loadSceneMode);
        }
    }
}