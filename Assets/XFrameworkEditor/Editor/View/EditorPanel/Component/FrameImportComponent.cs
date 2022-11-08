using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class FrameImportComponent : BaseEditor
    {
        [HorizontalGroup("地址")] [LabelText("包地址")]
        public string ComponentPackageServerPath;

        [LabelText("框架模块")] [VerticalGroup("框架模块")] [TableList(AlwaysExpanded = true, IsReadOnly = true)]
        public List<FrameImportComponentData> FrameComponentData = new List<FrameImportComponentData>();

        private static List<string> _directoryPath = new List<string>();
        private List<FrameComponentImportData> _frameComponentImportData = new List<FrameComponentImportData>();
        private FrameComponentEditorData _frameComponentEditorData;

        [HorizontalGroup("地址")]
        [Button("刷新")]
        public void Refresh()
        {
            if (Directory.Exists(ComponentPackageServerPath))
            {
                RefreshComponent();
            }
        }

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _frameComponentEditorData = AssetDatabase.LoadAssetAtPath<FrameComponentEditorData>(General.frameComponentEditorDataPath);
            if (_frameComponentEditorData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FrameComponentEditorData>(), General.frameComponentEditorDataPath);
                //读取数据
                _frameComponentEditorData = AssetDatabase.LoadAssetAtPath<FrameComponentEditorData>(General.frameComponentEditorDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _frameComponentEditorData.componentPackageServerPath = ComponentPackageServerPath;
            //标记脏区
            EditorUtility.SetDirty(_frameComponentEditorData);
        }

        /// <summary>
        /// 刷新资源
        /// </summary>
        private void RefreshComponent()
        {
            _frameComponentImportData.Clear();
            _directoryPath = new List<string>(Directory.GetDirectories(ComponentPackageServerPath));

            foreach (string directory in _directoryPath)
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    if (file.Contains("json"))
                    {
                        FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate);
                        StreamReader sr = new StreamReader(fileStream);
                        string textData = sr.ReadToEnd();
                        _frameComponentImportData.Add(JsonUtility.FromJson<FrameComponentImportData>(textData));
                        sr.Close();
                    }
                }
            }

            FrameComponentData.Clear();
            foreach (FrameComponentImportData frameComponentImportData in _frameComponentImportData)
            {
                List<string> allScripts = DataComponent.GetAllScriptsNameOnlyInAssetsPath();
                bool localScripts = false;
                for (int i = 0; i < allScripts.Count; i++)
                {
                    if (allScripts[i].Equals(frameComponentImportData.packageScriptName + ".cs"))
                    {
                        localScripts = true;
                        break;
                    }
                }

                FrameComponentData.Add(new FrameImportComponentData()
                {
                    importState = localScripts ? XFramework.FrameImportComponentData.ImportState.重新导入 : XFramework.FrameImportComponentData.ImportState.导入,
                    packageName = frameComponentImportData.packageName
                });
            }
        }

        public override void OnLoadConfig()
        {
            ComponentPackageServerPath = _frameComponentEditorData.componentPackageServerPath;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }


        public static void Import(string packageName)
        {
            foreach (string directory in _directoryPath)
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    if (file.Contains(packageName + ".unitypackage"))
                    {
                        AssetDatabase.ImportPackage(file, true);
                        return;
                    }
                }
            }
        }
    }
}