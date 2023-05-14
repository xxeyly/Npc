#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace XFramework
{
    public class CreateTemplateScript : EndNameEditAction
    {
        private GenerateBaseWindowData _generateBaseWindowData;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var className = Path.GetFileNameWithoutExtension(pathName);
            _generateBaseWindowData =
                AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            className = className.Replace(" ", "");


            text = text.Replace("#region StartUsing", _generateBaseWindowData.startUsing);
            text = text.Replace("#endregion EndUsing", _generateBaseWindowData.endUsing);
            text = text.Replace("#region StartUIVariable", _generateBaseWindowData.startUiVariable);
            text = text.Replace("#endregion EndUIVariable", _generateBaseWindowData.endUiVariable);
            text = text.Replace("#region StartVariableBindPath", _generateBaseWindowData.startVariableBindPath);
            text = text.Replace("#endregion EndVariableBindPath", _generateBaseWindowData.endVariableBindPath);
            text = text.Replace("#region StartVariableBindListener", _generateBaseWindowData.startVariableBindListener);
            text = text.Replace("#endregion EndVariableBindListener", _generateBaseWindowData.endVariableBindListener);
            text = text.Replace("#region StartVariableBindEvent", _generateBaseWindowData.startVariableBindEvent);
            text = text.Replace("#endregion EndVariableBindEvent", _generateBaseWindowData.endVariableBindEvent);

            if (resourceFile == General.BaseWindowTemplatePath)
            {
                text = text.Replace("BaseWindowTemplate", className);
                text = text.Replace("#region StartCustomAttributesStart", _generateBaseWindowData.startCustomAttributesStart);
                text = text.Replace("#endregion EndCustomAttributesStart", _generateBaseWindowData.endCustomAttributesStart);
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