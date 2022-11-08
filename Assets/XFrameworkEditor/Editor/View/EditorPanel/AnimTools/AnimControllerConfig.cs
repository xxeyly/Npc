#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace XFramework
{
    public class AnimControllerConfig : ScriptableObject
    {
        [LabelText("动画控制器")] public AnimatorController LoadAnimatorController;
        [LabelText("动画控制器名字")] public string LoadAnimatorControllerName;
        [LabelText("输出表文件夹")] [FolderPath] public string LoadExportTablePath;
        [LabelText("输出控制器文件夹")] [FolderPath] public string LoadExportControllerPath;

        public List<AnimFbxConfig> animFbxConfig;

        [Serializable]
        [LabelText("动画片段配置")]
        public class AnimFbxConfig
        {
            [LabelText("分割的动画文件")] [VerticalGroup("动画分割数据")]
            public GameObject animFbx;

            [LabelText("动画分割数据")] [VerticalGroup("动画分割数据")] [TableList(AlwaysExpanded = true)]
            public List<AnimClipSplitData> animClipSplitData;
        }

        [Serializable]
        [LabelText("动画分帧数据")]
        public class AnimClipSplitData
        {
            [HideLabel] [HorizontalGroup("动画名字")] public string animatorClipName;

            [HideLabel] [HorizontalGroup("属性类型")]
            public AnimatorControllerParameterType animatorControllerParameterType =
                AnimatorControllerParameterType.Trigger;

            [HideLabel] [HorizontalGroup("固定")] public bool fixedDuration;

            [HideLabel] [HorizontalGroup("过渡持续时间")]
            public float transitionDuration;

            [HideLabel] [HorizontalGroup("开始帧")] public int animatorClipFirstFrame;
            [HideLabel] [HorizontalGroup("结束帧")] public int animatorClipLastFrame;
            [HideLabel] [HorizontalGroup("循环")] public bool animatorClipIsLoop;
            [HideLabel] [HorizontalGroup("倒放")] public bool animatorClipIsRewind;
        }
    }
}
#endif
