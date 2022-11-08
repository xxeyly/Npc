#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class CreateTemplate
    {
        [MenuItem("Assets/Create/XFramework/C# BaseWindow", false, 70)]
        public static void OnCreateBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/NewBaseWindow.cs", null,
                General.BaseWindowTemplatePath);
        }

        [MenuItem("Assets/Create/XFramework/C# ChildBaseWindow", false, 71)]
        public static void OnCreateChildBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/NewChildBaseWindow.cs", null,
                General.ChildBaseWindowTemplatePath);
        }

        [MenuItem("Assets/Create/XFramework/C# CircuitBaseData", false, 72)]
        public static void OnCreateCircuitBaseDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/NewCircuitBaseData.cs", null,
                General.CircuitBaseDataTemplatePath);
        }

        [MenuItem("Assets/Create/XFramework/C# ListenerComponentData", false, 73)]
        public static void OnCreateListenerComponentDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/ListenerComponentData.cs", null,
                General.ListenerComponentDataTemplatePath);
        }

        [MenuItem("Assets/Create/XFramework/C# SceneComponent", false, 74)]
        public static void OnCreateSceneComponentTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/NewSceneComponent.cs", null,
                General.SceneComponentTemplatePath);
        }
        
        [MenuItem("Assets/Create/XFramework/C# SceneComponentInit", false, 75)]
        public static void OnCreateSceneComponentInitTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/NewSceneComponentInit.cs", null,
                General.SceneComponentInitTemplatePath);
        }


        [MenuItem("Assets/Create/XFramework/C# AnimatorControllerParameterData", false, 76)]
        public static void OnCreateAnimatorControllerParameterDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateTemplateScript>(), path + "/AnimatorControllerParameterData.cs", null,
                General.AnimatorControllerParameterDataTemplatePath);
        }

        /// <summary>
        /// 获得选择文件地址
        /// </summary>
        /// <returns></returns>
        private static string GetSelectedPath()
        {
            //默认路径为Assets
            string selectedPath = "Assets";

            //获取选中的资源
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            if (selection.Length != 1)
                return "";
            //遍历选中的资源以返回路径
            foreach (Object obj in selection)
            {
                selectedPath = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
                {
                    selectedPath = Path.GetDirectoryName(selectedPath);
                    break;
                }
            }

            return selectedPath;
        }
    }
}
#endif
