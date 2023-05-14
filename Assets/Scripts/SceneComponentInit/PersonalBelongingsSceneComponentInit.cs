using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

[InfoBox("背包格子初始化")]
public class PersonalBelongingsSceneComponentInit : SceneComponentInit
{
    public override void InitComponent()
    {
        ListenerFrameComponent.Instance.personalBelongings.InitStorageItem();
    }
}