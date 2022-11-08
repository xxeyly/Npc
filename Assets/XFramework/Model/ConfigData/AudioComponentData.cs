using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class AudioComponentData : ScriptableObject
    {
        [Searchable] [TableList(AlwaysExpanded = true)] [LabelText("音频内容")]
        public List<AudioInfo> audioInfos = new List<AudioInfo>();

        [Serializable]
        public struct AudioInfo
        {
            [HideLabel] [HorizontalGroup("名称")] public string audioName;
            [HideLabel] [HorizontalGroup("片段")] public AudioClip audioClip;
        }
    }
}