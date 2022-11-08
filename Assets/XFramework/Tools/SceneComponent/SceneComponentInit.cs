using UnityEngine;
using XFramework;

namespace XFramework
{
    public abstract partial class SceneComponentInit : MonoBehaviour, ISceneComponent
    {
        public void StartComponent()
        {
        }

        public abstract void InitComponent();
    }
}