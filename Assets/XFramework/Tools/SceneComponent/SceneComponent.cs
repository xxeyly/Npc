using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract partial class SceneComponent : SerializedMonoBehaviour, ISceneComponent
    {
        public abstract void StartComponent();
        public abstract void InitComponent();
        public abstract void EndComponent();

        public void AddToSceneComponent()
        {
            if (!GameRootStart.Instance.sceneStartSingletons.Contains(this))
            {
                GameRootStart.Instance.sceneStartSingletons.Add(this);
                StartComponent();
                InitComponent();
            }
            
        }
    }
}