#if UNITY_EDITOR

using System;
using System.IO;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class FrameMenu : OdinMenuEditorWindow
    {
        private FrameMenu()
        {
        }


        [MenuItem("Xframe/框架界面")]
        private static void OpenWindow()
        {
            GetWindow<FrameMenu>().Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _audioComponentEditor.OnDisable();
            gameRootEditor.OnDisable();
            _frameImportComponent.OnDisable();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Xframe/监听生成 &l")]
        private static void OnListenerGenerate()
        {
            /*ListenerComponentGenerateData listenerComponentGenerateData = new ListenerComponentGenerateData();
            listenerComponentGenerateData.OnGenerate();*/
            GenerateListenerComponent.GenerateListener();
        }

        [MenuItem("Xframe/生成代码配置")]
        private static void OnGenerateBaseWindowData()
        {
            GenerateBaseWindowData generateBaseWindowData = AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            if (generateBaseWindowData == null)
            {
                if (!Directory.Exists(General.generateBaseWindowPath))
                {
                    Directory.CreateDirectory(General.generateBaseWindowPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GenerateBaseWindowData>(), General.generateBaseWindowPath);
            }
        }

        [MenuItem("Xframe/生成框架 &F")]
        public static void Generate()
        {
            GameObject gameRootStart = GameObject.Find("GameRootStart");
            if (gameRootStart != null)
            {
                DestroyImmediate(gameRootStart);
            }

            gameRootStart = new GameObject("GameRootStart");
            GameRootStart tempGameRootStart = gameRootStart.AddComponent<GameRootStart>();
            //添加框架组件
            Undo.RegisterCreatedObjectUndo(gameRootStart, "UndoCreate");
            foreach (Type type in General.frameComponentType)
            {
                GameObject tempComponentObj = new GameObject(type.Name);
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempComponentObj.AddComponent(type);
                // tempGameRootStart.frameComponent.Add(tempComponentObj.GetComponent<FrameComponent>());
            }
        }

        //打包
        // private static CustomBuild customBuild = new CustomBuild();

        //音频组件
        private AudioComponentEditor _audioComponentEditor = new AudioComponentEditor();

        //框架配置
        private static GameRootEditor gameRootEditor = new GameRootEditor();

        //资源统一化
        ResourceUnification resourceUnification = new ResourceUnification();

        //动画工具
        AnimTools animTools = new AnimTools();
        FrameImportComponent _frameImportComponent = new FrameImportComponent();

        //生成配置
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            _audioComponentEditor.OnInit();
            resourceUnification.OnInit();
            _frameImportComponent.OnInit();
            tree.Add("导出框架", gameRootEditor);
            tree.Add("音频配置", _audioComponentEditor);
            tree.Add("资源统一化", resourceUnification);
            tree.Add("动画工具", animTools);
            tree.Add("框架组件", _frameImportComponent);
            return tree;
        }
    }
}
#endif