using System;
using Sirenix.OdinInspector;
using UnityEditor;

[Serializable]
public class HotFixAssetPrefabsConfig
{
#if UNITY_EDITOR
    [LabelText("场景文件")] public SceneAsset screen;
    [LabelText("预制体文件夹路径")] [FolderPath] public string prefabFolderPath;
    [LabelText("打包AB包文件夹路径")] [FolderPath] public string assetBundleFolderPath;
#endif
}