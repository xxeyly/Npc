using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class HotFixAssetSceneHierarchyPath : MonoBehaviour
{
    [LabelText("路径")] public string path;

    [Button("查找路径", ButtonSizes.Medium)]
    [LabelText("重命名")]
    [GUIColor(0, 1, 0)]
    public void SetHierarchyPath()
    {
        path = DataFrameComponent.GetComponentPath(transform, false);
    }
}