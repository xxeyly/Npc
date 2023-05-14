using Sirenix.OdinInspector;
using XFramework;

[InfoBox("物品图鉴初始化")]
public class ItemAtlasDisplayInit : SceneComponentInit
{
    public override void InitComponent()
    {
        ListenerFrameComponent.Instance.itemAtlasDisplay.InitAtlas();
    }
}