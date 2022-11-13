using Sirenix.OdinInspector;
using XFramework;

[InfoBox("小型制药器初始化")]
public class SmallPharmaceuticalApparatusSceneComponentInit : SceneComponentInit
{
    public override void InitComponent()
    {
        ListenerComponent.Instance.smallPharmaceuticalApparatus.InitStorageItem();
    }
}