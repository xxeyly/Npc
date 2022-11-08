#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;
using Object = UnityEngine.Object;

namespace XFramework
{
    public class AnimTools : BaseEditor
    {
        [TabGroup("AnimConfig", "加载配置")] [AssetList] [LabelText("动画配置文件")] [OnValueChanged("AnimatorControllerOnValueChanged")]
        public AnimControllerConfig AnimControllerConfig;

        [TabGroup("AnimConfig", "加载配置")] [LabelText("动画控制器")] [ReadOnly]
        public AnimatorController LoadAnimatorController;

        [TabGroup("AnimConfig", "加载配置")] [LabelText("动画控制器名字")] [ReadOnly]
        public string LoadAnimatorControllerName;

        [TabGroup("AnimConfig", "加载配置")] [LabelText("输出表文件夹")] [FolderPath] [ReadOnly]
        public string LoadExportTablePath;

        [TabGroup("AnimConfig", "加载配置")] [LabelText("输出控制器文件夹")] [FolderPath] [ReadOnly]
        public string LoadExportControllerPath;

        [TabGroup("AnimConfig", "加载配置")] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("动画配置内容")]
        public List<AnimControllerConfig.AnimFbxConfig> AnimClipConfigs;

        private ModelImporter _modelImporter;

        [TabGroup("AnimConfig", "新建配置")] [LabelText("动画配置表名字")]
        public string NewAnimControllerConfigName;

        [TabGroup("AnimConfig", "新建配置")] [LabelText("动画控制器名字")]
        public string NewAnimatorControllerName;

        [TabGroup("AnimConfig", "新建配置")] [LabelText("输出配置表文件夹")] [FolderPath]
        public string NewExportConfigPath;

        [TabGroup("AnimConfig", "新建配置")] [LabelText("输出动画控制器文件夹")] [FolderPath]
        public string NewExportControllerPath;

        [TabGroup("AnimConfig", "新建配置")]
        [LabelText("新建动画配置文件")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void OnCreateAnimControllerConfig()
        {
            if (NewAnimControllerConfigName == String.Empty || NewExportConfigPath == String.Empty)
            {
                Debug.Log("文件名为空");
                return;
            }

            if (AssetDatabase.LoadAssetAtPath<AnimControllerConfig>(
                    NewExportConfigPath + "/" + NewAnimControllerConfigName) != null)
            {
                Debug.Log("已存在相同项目");
            }
            else
            {
                //创建配置表
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnimControllerConfig>(), NewExportConfigPath + "/" + NewAnimControllerConfigName + ".asset");
                //加载配置表
                AnimControllerConfig = AssetDatabase.LoadAssetAtPath<AnimControllerConfig>(NewExportConfigPath + "/" + NewAnimControllerConfigName + ".asset");
                //创建控制器
                AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(NewExportControllerPath + "/" + NewAnimatorControllerName + ".controller");
                //初始化数据
                AnimControllerConfig.animFbxConfig = new List<AnimControllerConfig.AnimFbxConfig>();
                AnimControllerConfig.LoadAnimatorControllerName = NewAnimatorControllerName;
                AnimControllerConfig.LoadExportTablePath = NewExportConfigPath;
                AnimControllerConfig.LoadExportControllerPath = NewExportControllerPath;
                AnimControllerConfig.LoadAnimatorController = animatorController;
                //保存数据
                EditorUtility.SetDirty(AnimControllerConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        public void AnimatorControllerOnValueChanged()
        {
            if (AnimControllerConfig != null)
            {
                LoadAnimatorController = AnimControllerConfig.LoadAnimatorController;
                LoadAnimatorControllerName = AnimControllerConfig.LoadAnimatorControllerName;
                LoadExportTablePath = AnimControllerConfig.LoadExportTablePath;
                LoadExportControllerPath = AnimControllerConfig.LoadExportControllerPath;
                AnimClipConfigs = AnimControllerConfig.animFbxConfig;
            }
            else
            {
                AnimClipConfigs = null;
            }
        }

        #region 构建动画片段键值对

        private Dictionary<string, AnimationClip> BuildingAnimationClips(List<Object> allFbxObject,
            List<string> animClipNames)
        {
            Dictionary<string, AnimationClip> animationClipDic = new Dictionary<string, AnimationClip>();
            AnimationClip animationClip;
            foreach (string animClipName in animClipNames)
            {
                foreach (Object objectClip in allFbxObject)
                {
                    if (objectClip.name == animClipName)
                    {
                        animationClip = objectClip as AnimationClip;
                        animationClipDic.Add(animClipName, animationClip);
                        break;
                    }
                }
            }

            return animationClipDic;
        }

        /// <summary>
        /// 获得所有动画片段的名字
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllAnimClipName()
        {
            List<string> animNames = new List<string>();
            foreach (AnimControllerConfig.AnimFbxConfig animFbxAndAnimClipData in AnimControllerConfig.animFbxConfig)
            {
                foreach (AnimControllerConfig.AnimClipSplitData animClipSplitData in animFbxAndAnimClipData.animClipSplitData)
                {
                    animNames.Add(animClipSplitData.animatorClipName);
                }
            }

            return animNames;
        }

        #endregion

        #region 设置动画片段

        private ModelImporterClipAnimation SetClipAnimation(string clipName, int firstFrame, int lastFrame, bool isLoop)
        {
            ModelImporterClipAnimation clip = new ModelImporterClipAnimation { name = clipName, firstFrame = firstFrame, lastFrame = lastFrame, loopTime = isLoop };

            if (isLoop)
            {
                clip.wrapMode = WrapMode.Loop;
            }
            else
            {
                clip.wrapMode = WrapMode.Default;
            }

            return clip;
        }

        #endregion

        [TabGroup("AnimConfig", "加载配置")]
        [LabelText("生成动画配置文件")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void BuildAnim()
        {
            if (!Directory.Exists(LoadExportTablePath) || !Directory.Exists(LoadExportControllerPath) || LoadAnimatorControllerName == string.Empty)
            {
                return;
            }

            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(LoadExportControllerPath + "/" + LoadAnimatorControllerName + ".controller");
            AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;
            foreach (AnimControllerConfig.AnimFbxConfig animFbxAndAnimClipData in AnimClipConfigs)
            {
                _modelImporter = (ModelImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(animFbxAndAnimClipData.animFbx));
                if (_modelImporter != null)
                {
                    _modelImporter.animationType = ModelImporterAnimationType.Generic;
                    _modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
                    ModelImporterClipAnimation[] animations = new ModelImporterClipAnimation[animFbxAndAnimClipData.animClipSplitData.Count];

                    for (int i = 0; i < animFbxAndAnimClipData.animClipSplitData.Count; i++)
                    {
                        animations[i] = SetClipAnimation(
                            animFbxAndAnimClipData.animClipSplitData[i].animatorClipName,
                            animFbxAndAnimClipData.animClipSplitData[i].animatorClipFirstFrame,
                            animFbxAndAnimClipData.animClipSplitData[i].animatorClipLastFrame,
                            animFbxAndAnimClipData.animClipSplitData[i].animatorClipIsLoop);
                    }

                    _modelImporter.clipAnimations = animations;
                    _modelImporter.SaveAndReimport();
                    //该动画文件下的所有文件
                    List<Object> allAnimObject = new List<Object>(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(animFbxAndAnimClipData.animFbx)));
                    List<string> allAnimClipName = GetAllAnimClipName();
                    Dictionary<string, AnimationClip> animationClipDic = BuildingAnimationClips(allAnimObject, allAnimClipName);

                    for (int i = 0; i < animFbxAndAnimClipData.animClipSplitData.Count; i++)
                    {
                        //添加 参数
                        animatorController.AddParameter(animFbxAndAnimClipData.animClipSplitData[i].animatorClipName, animFbxAndAnimClipData.animClipSplitData[i].animatorControllerParameterType);
                        //添加 片段
                        AnimatorState state = rootStateMachine.AddState(animFbxAndAnimClipData.animClipSplitData[i].animatorClipName);
                        //动画是否倒放
                        state.speed = animFbxAndAnimClipData.animClipSplitData[i].animatorClipIsRewind ? -1 : 1;
                        //设置动画
                        state.motion = animationClipDic[animFbxAndAnimClipData.animClipSplitData[i].animatorClipName];
                        // 关联片段 
                        AnimatorStateTransition animatorStateTransition = rootStateMachine.AddAnyStateTransition(state);
                        //设置关联参数
                        animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, animFbxAndAnimClipData.animClipSplitData[i].animatorClipName);
                        //设置持续时间
                        animatorStateTransition.duration = animFbxAndAnimClipData.animClipSplitData[i].transitionDuration;
                        animatorStateTransition.hasFixedDuration = animFbxAndAnimClipData.animClipSplitData[i].fixedDuration;
                    }

                    LoadAnimatorController = animatorController;
                    AnimControllerConfig.LoadAnimatorController = animatorController;
                    EditorUtility.SetDirty(AnimControllerConfig);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError("当前Fbx文件没有选择");
                }
            }
        }

        public override void OnDisable()
        {
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
        }
    }
}
#endif