
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        protected void SceneLoad(string sceneName)
        {
            SceneLoadFrameComponent.Instance.SceneLoad(sceneName);
        }
    }
}