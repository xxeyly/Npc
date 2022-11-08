#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace XFramework
{
    public class CreateTemplateScript : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var className = Path.GetFileNameWithoutExtension(pathName);
            className = className.Replace(" ", "");
            
            text = text.Replace("StartUsing", GenerateBaseWindowData.startUsing);
            text = text.Replace("EndUsing", GenerateBaseWindowData.endUsing);
            text = text.Replace("StartUIVariable", GenerateBaseWindowData.startUiVariable);
            text = text.Replace("EndUIVariable", GenerateBaseWindowData.endUiVariable);
            text = text.Replace("StartVariableBindPath", GenerateBaseWindowData.startVariableBindPath);
            text = text.Replace("EndVariableBindPath", GenerateBaseWindowData.endVariableBindPath);
            text = text.Replace("StartVariableBindListener", GenerateBaseWindowData.startVariableBindListener);
            text = text.Replace("EndVariableBindListener", GenerateBaseWindowData.endVariableBindListener);
            text = text.Replace("StartVariableBindEvent", GenerateBaseWindowData.startVariableBindEvent);
            text = text.Replace("EndVariableBindEvent", GenerateBaseWindowData.endVariableBindEvent);

            if (resourceFile == General.BaseWindowTemplatePath)
            {
                text = text.Replace("BaseWindowTemplate", className);
                text = text.Replace("StartCustomAttributesStart", GenerateBaseWindowData.startCustomAttributesStart);
                text = text.Replace("EndCustomAttributesStart", GenerateBaseWindowData.endCustomAttributesStart);
            }

            if (resourceFile == General.ChildBaseWindowTemplatePath)
            {
                text = text.Replace("ChildBaseWindowTemplate", className);
            }

            if (resourceFile == General.CircuitBaseDataTemplatePath)
            {
                text = text.Replace("CircuitBaseDataTemplate", className);
            }

            if (resourceFile == General.ListenerComponentDataTemplatePath)
            {
                text = text.Replace("ListenerComponentDataTemplate", "ListenerComponent");
            }

            if (resourceFile == General.SceneComponentTemplatePath)
            {
                text = text.Replace("SceneComponentTemplate", className);
            }

            if (resourceFile == General.SceneComponentInitTemplatePath)
            {
                text = text.Replace("SceneComponentInitTemplate", className);
            }

            if (resourceFile == General.AnimatorControllerParameterDataTemplatePath)
            {
                text = text.Replace("AnimatorControllerParameterDataTemplate", "AnimatorControllerData");
            }


            //utf8
            var encoding = new UTF8Encoding(true, false);

            File.WriteAllText(pathName, text, encoding);

            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }
}
#endif