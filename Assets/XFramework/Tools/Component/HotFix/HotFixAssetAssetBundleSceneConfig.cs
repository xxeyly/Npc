using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
[InfoBox("场景内所有AssetBundle资源信息")]
public class HotFixAssetAssetBundleSceneConfig
{
    [LabelText("场景配置")] public HotFixAssetAssetBundleAssetConfig sceneHotFixAssetAssetBundleAssetConfig = new HotFixAssetAssetBundleAssetConfig();
    [LabelText("场景AssetBundle")] public List<HotFixAssetAssetBundleAssetConfig> assetBundleHotFixAssetAssetBundleAssetConfigs = new List<HotFixAssetAssetBundleAssetConfig>();
}