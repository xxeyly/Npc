using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XFramework;

public class HotFixAssetPrefabsConfigManager : SerializedMonoBehaviour
{
#if UNITY_EDITOR
    [LabelText("AssetBundle变体名称")] public string assetBundleVariant = "ab";

    [LabelText("场景预制体配置")] public List<HotFixAssetPrefabsConfig> hotFixAssetPrefabsConfigs = new List<HotFixAssetPrefabsConfig>();
    [LabelText("prefab实例化配置")] private Dictionary<string, string> hotFixAssetSceneHierarchyPathDict = new Dictionary<string, string>();
    [LabelText("热修复包AssetBundle配置")] public List<HotFixAssetAssetBundleSceneConfig> hotFixAssetAssetBundleSceneConfigs = new List<HotFixAssetAssetBundleSceneConfig>();

    [Button("打包AB包并生成配置")]
    public void BuildABAndGenerateConfig()
    {
        hotFixAssetAssetBundleSceneConfigs.Clear();
        //打AssetBundle
        for (int i = 0; i < hotFixAssetPrefabsConfigs.Count; i++)
        {
            hotFixAssetSceneHierarchyPathDict.Clear();
            //场景
            AssetImporter sceneAssetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(hotFixAssetPrefabsConfigs[i].screen));
            string sceneAssetBundleName = hotFixAssetPrefabsConfigs[i].assetBundleFolderPath.Replace("Assets/StreamingAssets/", "") + "/Scene/" + hotFixAssetPrefabsConfigs[i].screen.name;
            sceneAssetImporter.SetAssetBundleNameAndVariant(sceneAssetBundleName, assetBundleVariant);
            //文件夹
            BuildPrefab(hotFixAssetPrefabsConfigs[i].prefabFolderPath, hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "UI");
            BuildPrefab(hotFixAssetPrefabsConfigs[i].prefabFolderPath, hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "Env");
            BuildPrefab(hotFixAssetPrefabsConfigs[i].prefabFolderPath, hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "Entity");
            BuildPrefab(hotFixAssetPrefabsConfigs[i].prefabFolderPath, hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "SceneComponent");
            BuildPrefab(hotFixAssetPrefabsConfigs[i].prefabFolderPath, hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "SceneComponentInit");

            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            UnityEditor.AssetDatabase.Refresh();
            DataFrameComponent.RemoveAllAssetBundleName();

            HotFixAssetAssetBundleSceneConfig hotFixAssetAssetBundleSceneConfig = new HotFixAssetAssetBundleSceneConfig();
            //场景资源
            string sceneAssetBundlePath = GetSceneAssetBundlePath(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath + "/" + "Scene");
            //场景名称
            hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.assetBundleName = DataFrameComponent.GetPathFileNameDontContainFileType(sceneAssetBundlePath);
            hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.md5 = FileOperation.GetMD5HashFromFile(sceneAssetBundlePath);
            //场景加载路径
            hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.assetBundlePath =
                DataFrameComponent.GetPathDontContainFileName(DataFrameComponent.AllCharToLower(sceneAssetBundlePath.Replace("Assets/StreamingAssets/", "")));
            hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.assetBundleSize = FileOperation.GetFileSize(sceneAssetBundlePath);
            //UI
            foreach (HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig in BuildAssetBundle(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "UI", HotFixAssetType.UI))
            {
                hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
            }

            foreach (HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig in BuildAssetBundle(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "Env", HotFixAssetType.Env))
            {
                hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
            }

            foreach (HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig in BuildAssetBundle(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "Entity", HotFixAssetType.Entity))
            {
                hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
            }

            foreach (HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig in BuildAssetBundle(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "SceneComponent", HotFixAssetType.SceneComponent))
            {
                hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
            }

            foreach (HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig in BuildAssetBundle(hotFixAssetPrefabsConfigs[i].assetBundleFolderPath, "SceneComponentInit", HotFixAssetType.SceneComponentInit))
            {
                hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
            }

            hotFixAssetAssetBundleSceneConfigs.Add(hotFixAssetAssetBundleSceneConfig);
        }


        FileOperation.SaveTextToLoad(Application.streamingAssetsPath, "HotFixAssetConfig.json", JsonMapper.ToJson(hotFixAssetAssetBundleSceneConfigs));
    }

    //返回路径场景AssetBundle路径
    private string GetSceneAssetBundlePath(string path)
    {
        DirectoryInfo sceneDirection = new DirectoryInfo(path);
        foreach (FileInfo fileInfo in sceneDirection.GetFiles())
        {
            if (!fileInfo.Name.Contains(".manifest") && !fileInfo.Name.Contains(".meta"))
            {
                return path + "/" + fileInfo.Name;
            }
        }

        return string.Empty;
    }

    //返回路径内所有AssetBundle路径
    private List<string> GetAllAssetBundlePath(string path)
    {
        List<string> allAssetBundlePath = new List<string>();
        DirectoryInfo sceneDirection = new DirectoryInfo(path);
        if (!sceneDirection.Exists)
        {
            return allAssetBundlePath;
        }

        foreach (FileInfo fileInfo in sceneDirection.GetFiles())
        {
            if (!fileInfo.Name.Contains(".manifest") && !fileInfo.Name.Contains(".meta"))
            {
                allAssetBundlePath.Add(path + "/" + fileInfo.Name);
            }
        }

        return allAssetBundlePath;
    }

    //处理Prefab文件夹
    private void BuildPrefab(string prefabFolderPath, string assetBundleFolderPath, string folder)
    {
        //UI文件夹
        DirectoryInfo prefabDirection = new DirectoryInfo(prefabFolderPath + "/" + folder);
        //空文件夹不做处理
        if (prefabDirection.GetFiles().Length == 0)
        {
            return;
        }

        DataFrameComponent.SetFolderFileBuildAssetBundleName(prefabFolderPath + "/" + folder, assetBundleFolderPath + "/" + folder, assetBundleVariant);
        foreach (FileInfo fileInfo in prefabDirection.GetFiles())
        {
            if (fileInfo.Name.Contains(".prefab") && !fileInfo.Name.Contains(".meta"))
            {
                try
                {
                    string prefabPath = prefabFolderPath + "/" + folder + "/" + fileInfo.Name;
                    HotFixAssetSceneHierarchyPath hotFixAssetSceneHierarchyPath = AssetDatabase.LoadAssetAtPath<HotFixAssetSceneHierarchyPath>(prefabPath);
                    hotFixAssetSceneHierarchyPathDict.Add(DataFrameComponent.AllCharToLower(DataFrameComponent.GetPathFileNameDontContainFileType(fileInfo.Name)), hotFixAssetSceneHierarchyPath.path);
                }
                catch (Exception e)
                {
                    Debug.Log(prefabFolderPath + "/" + folder + "/" + fileInfo.Name + e);
                }
            }
        }
    }

    //处理AssetBundle文件夹
    private List<HotFixAssetAssetBundleAssetConfig> BuildAssetBundle(string assetBundleFolderPath, string folder, HotFixAssetType hotFixAssetType)
    {
        List<HotFixAssetAssetBundleAssetConfig> hotFixAssetAssetBundleAssetConfigs = new List<HotFixAssetAssetBundleAssetConfig>();
        List<string> prefabPath = GetAllAssetBundlePath(assetBundleFolderPath + "/" + folder);
        foreach (string path in prefabPath)
        {
            HotFixAssetAssetBundleAssetConfig hotFixAssetAssetBundleAssetConfig = new HotFixAssetAssetBundleAssetConfig();
            hotFixAssetAssetBundleAssetConfig.assetBundleName = DataFrameComponent.GetPathFileNameDontContainFileType(path);
            hotFixAssetAssetBundleAssetConfig.assetBundleType = hotFixAssetType;
            hotFixAssetAssetBundleAssetConfig.md5 = FileOperation.GetMD5HashFromFile(path);
            hotFixAssetAssetBundleAssetConfig.assetBundlePath = DataFrameComponent.GetPathDontContainFileName(DataFrameComponent.AllCharToLower(path.Replace("Assets/StreamingAssets/", "")));
            hotFixAssetAssetBundleAssetConfig.assetBundleSize = FileOperation.GetFileSize(path);
            hotFixAssetAssetBundleAssetConfig.assetBundleInstantiatePath = hotFixAssetSceneHierarchyPathDict[DataFrameComponent.GetPathFileNameDontContainFileType(path)];
            hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleAssetConfig);
        }

        return hotFixAssetAssetBundleAssetConfigs;
    }
#endif
    [Button("清空所有AB包名称")]
    public void ClearAllAssetBundleName()
    {
        DataFrameComponent.RemoveAllAssetBundleName();
    }
}