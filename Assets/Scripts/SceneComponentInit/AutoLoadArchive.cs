using XFramework;

public class AutoLoadArchive : SceneComponentInit
{
    public override void InitComponent()
    {
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.Load();
    }
}