#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    [Serializable]
    public class CustomBuildData : ScriptableObject
    {
        [LabelText("当前打包方式:")] public General.BuildTargetPlatform buildTargetPlatform;
        [LabelText("压缩类型")] public BuildOptions buildCompressType;

        [LabelText("当前打包存放路径")] [FolderPath(AbsolutePath = true)]
        public string buildPackagePath;

        [LabelText("中文名称")] [Tooltip("只做输出外壳使用")]
        public string exportCnProjectName;

        [LabelText("英文名称")] public string exportEnProjectName;

        [LabelText("使用中文输出外壳")] public bool chineseShell;
        [LabelText("文件拷贝输出路径")] public List<FolderCopy> folderCopy;
    }
}
#endif
