using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    [Serializable]
    [LabelText("动画分帧数据")]
    public class FrameImportComponentData
    {
        public enum ImportState
        {
            导入,
            重新导入
        }

        [HideLabel] [HorizontalGroup("包体名字")] [ReadOnly]
        public string packageName;

        /*[HideLabel] [HorizontalGroup("导入状态")]*/
        [HideInInspector] public ImportState importState;

        [HorizontalGroup("")]
        [Button("导入")]
        [ShowInInspector]
        [EnableIf("importState", ImportState.导入)]
        public void Import()
        {
            FrameImportComponent.Import(packageName);
        }

        [HorizontalGroup("")]
        [Button("重新导入")]
        [ShowInInspector]
        public void ReImport()
        {
            FrameImportComponent.Import(packageName);
        }
    }

    [Serializable]
    public class FrameComponentImportData
    {
        public string packageName;
        public string packageScriptName;
    }
}