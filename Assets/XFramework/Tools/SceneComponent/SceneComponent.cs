using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract partial class SceneComponent : SerializedMonoBehaviour, ISceneComponent
    {
        public abstract void StartComponent();
        public abstract void EndComponent();

        public void AddToSceneComponent()
        {
            if (GameRootStart.Instance.sceneComponents.Contains(this))
            {
                return;
            }

            GameRootStart.Instance.sceneComponents.Add(this);
            StartComponent();
        }
    }
}