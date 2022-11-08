#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 已打包场景AB包数据
    /// 记录打包信息
    /// </summary>
    [Serializable]
    public class BuildSceneAssetBundleEditorData : ScriptableObject
    {
        [LabelText("场景Build文件")] public List<SceneAssetBundleInfo> sceneAssetBundleInfos;

        [Serializable]
        public class SceneAssetBundleInfo
        {
            [LabelText("场景文件")] public SceneAsset sceneAsset;
            [LabelText("文件Md5")] public string Md5;
            [LabelText("场景AB包地址")] public string sceneAbPath;
        }
    }
}
#endif
