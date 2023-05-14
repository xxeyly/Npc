using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract class FrameComponent : SerializedMonoBehaviour, IComponent
    {
        public abstract void FrameInitComponent();
        public abstract void FrameSceneInitComponent();
        public abstract void FrameEndComponent();
    }
}