using Sirenix.OdinInspector;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using XFramework;

public class AttributeEditor : BaseEditor
{
    [LabelText("物品图鉴")] [SerializeField] [Searchable] [TableList(AlwaysExpanded = true)] [InlineEditor()]
    private ItemAtlas itemAtlas;

    public override void OnDisable()
    {
    }

    public override void OnCreateConfig()
    {
    }

    public override void OnSaveConfig()
    {
    }

    public override void OnLoadConfig()
    {
    }

    public override void OnInit()
    {
    }
}