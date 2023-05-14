using System;
using Sirenix.OdinInspector;

[Serializable]
public class HotFixAssetAssetBundleAssetConfig
{
    [LabelText("AssetBundle名称")] public string assetBundleName;
    [LabelText("AssetBundle类型")] public HotFixAssetType assetBundleType;
    [LabelText("AssetBundle路径")] public string assetBundlePath;

    [LabelText("AssetBundle实例化路径")] public string assetBundleInstantiatePath;
    [LabelText("Md5码")] public string md5;
    [LabelText("AssetBundle大小")] public long assetBundleSize;
}

[LabelText("资源类型")]
public enum HotFixAssetType
{
    Scene,
    Env,
    UI,
    Entity,
    SceneComponent,
    SceneComponentInit
}