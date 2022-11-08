using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract class FrameComponent : MonoBehaviour, IComponent
    {
        public abstract void FrameInitComponent();
        public abstract void FrameSceneInitComponent();
        public abstract void FrameEndComponent();
    }
}