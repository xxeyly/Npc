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
            DirectoryInfo directoryInfo = new DirectoryInfo(ComponentPackageServerPath);
            FrameComponentData.Clear();

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name.Contains(".unitypackage"))
                {
                    FrameComponentData.Add(new FrameImportComponentData()
                    {
                        importState = FrameImportComponentData.ImportState.导入,
                        packageName = DataFrameComponent.GetPathFileNameDontContainFileType(file.Name),
                        importPath = file.FullName
                    });
                }
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


        public static void Import(string packagePath)
        {
            AssetDatabase.ImportPackage(packagePath, true);
        }
    }
}