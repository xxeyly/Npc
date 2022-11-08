#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 打包数据
    /// </summary>
    [Serializable]
    public class CustomBuildEditorData : ScriptableObject
    {
        /// <summary>
        /// 平台
        /// </summary>
        [Header("目标平台")] public BuildTarget buildTarget;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        [Header("输出文件夹")] public string exportPath;

        /// <summary>
        /// 项目英文名称
        /// </summary>
        [Header("项目英文名称")] public string exportEnProjectName;

        /// <summary>
        /// 项目中文名称
        /// </summary>
        [Header("项目英文名称")] public string exportCnProjectName;

        /// <summary>
        /// 项目日期
        /// </summary>
        [Header("项目日期")] public bool projectNameDate;

        /// <summary>
        /// 中文外壳
        /// </summary>
        [Header("学习模式")] public bool chineseShell;

        /// <summary>
        /// 学习模式
        /// </summary>
        [Header("学习模式")] public bool learningModel;

        /// <summary>
        /// 考核模式
        /// </summary>
        [Header("考核模式")] public bool assessmentMode;

        /// <summary>
        /// 水印
        /// </summary>
        [Header("水印")] public bool watermark;

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        [Header("拷贝文件夹")] public List<FolderCopy> folderCopies = new List<FolderCopy>();

        /// <summary>
        /// 更新到服务器
        /// </summary>
        [Header("更新到服务器")] public bool updateToFtp;


        /// <summary>
        /// 服务器地址
        /// </summary>
        [Header("服务器地址")] public string ftpServerPath;

        /// <summary>
        /// Ftp用户名
        /// </summary>
        [Header("目标平台")] public string ftpUser;

        /// <summary>
        /// Ftp密码
        /// </summary>
        [Header("Ftp密码")] public string ftpPwd;

        /// <summary>
        /// Ftp根目录
        /// </summary>
        [Header("Ftp根目录")] public string ftpRoot;

        /// <summary>
        /// 版本设置
        /// </summary>
        [Header("版本设置")] public bool versionSet;

        /// <summary>
        /// 版本水印
        /// </summary>
        [Header("版本水印")] public bool versionWatermark;

        /// <summary>
        /// 下载
        /// </summary>
        [Header("版本下载")] public bool versionDownLoad;

        /// <summary>
        /// 加载进度
        /// </summary>
        [Header("版本下载进度")] public bool versionLoadingProgress;

        /// <summary>
        /// 场景进度
        /// </summary>
        [Header("版本场景进度")] public bool versionSceneProgress;

        /// <summary>
        /// 版本考核时间
        /// </summary>
        [Header("版本考核时间")] public int versionAssessmentTime;
    }

    /// <summary>
    /// 文件夹拷贝
    /// </summary>
    [Serializable]
    public class FolderCopy
    {
        /// <summary>
        /// 拷贝文件夹路径
        /// </summary>
        [LabelText("拷贝文件夹路径")] [FolderPath] public string copyFolderPath;

        /// <summary>
        /// 粘贴文件夹根路径
        /// </summary>
        [LabelText("粘贴文件夹路径")] public string pasteFolderPath;
    }
}
#endif
