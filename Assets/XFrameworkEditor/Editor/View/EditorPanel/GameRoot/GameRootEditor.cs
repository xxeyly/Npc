#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class GameRootEditor : BaseEditor
    {
        [BoxGroup("Export")] [LabelText("导出路径")] [FolderPath(AbsolutePath = true)]
        public string ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        [BoxGroup("Export")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        [LabelText("导出框架")]
        public void ExportFrameToDesktop()
        {
            AssetDatabase.ExportPackage("Assets/" + "XFramework", ExportPath + "/XFramework.unitypackage", ExportPackageOptions.Recurse);
        }

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}
#endif