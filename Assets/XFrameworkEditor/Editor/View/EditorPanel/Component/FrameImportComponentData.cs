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
        }
        [HideLabel] [HorizontalGroup("包体名字")] [ReadOnly]
        public string packageName;
        [HideLabel][HideInInspector]
        public string importPath;
        /*[HideLabel] [HorizontalGroup("导入状态")]*/
        [HideInInspector] public ImportState importState;

        [HorizontalGroup("")]
        [Button("导入")]
        [ShowInInspector]
        [EnableIf("importState", ImportState.导入)]
        public void Import()
        {
            FrameImportComponent.Import(importPath);
        }
       
    }
   
}