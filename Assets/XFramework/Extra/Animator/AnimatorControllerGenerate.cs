using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class AnimatorControllerGenerate : MonoBehaviour
    {
#if UNITY_EDITOR
        [LabelText("加载脚本路径")] public string loadScriptsPath = "Scripts";
        [LabelText("加载脚本名称")] public string loadScriptsName = "AnimatorControllerParameterData";

        /// <summary>
        /// 获得生成内容
        /// </summary>
        /// <returns></returns>
        public List<string> GetReplaceContent()
        {
            List<string> replaceContent = new List<string>();
            List<string> animatorPath = DataFrameComponent.GetSpecifyTypeOnlyInAssetsPath("controller");
            List<RuntimeAnimatorController> animators = DataFrameComponent.GetSpecifyTypeOnlyInAssetsByFilePath<RuntimeAnimatorController>(animatorPath);
            foreach (RuntimeAnimatorController animator in animators)
            {
                foreach (AnimationClip animatorAnimationClip in animator.animationClips)
                {
                    if (animatorAnimationClip.name != string.Empty && !animatorAnimationClip.name.Contains(" ") && !replaceContent.Contains(animatorAnimationClip.name))
                    {
                        replaceContent.Add(animatorAnimationClip.name);
                    }
                }
            }

            return replaceContent;
        }

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void Generate()
        {
            string listenerComponentDataPath = GenerateGeneral.GetPath(loadScriptsName);
            if (listenerComponentDataPath == null)
            {
                Debug.LogWarning("AnimatorControllerData脚本未创建");
                return;
            }

            #region 加载本地脚本

            //脚本路径
            string scriptsPath = Application.dataPath + "/" + loadScriptsPath;

            //获取指定路径下面的所有资源文件  
            if (Directory.Exists(scriptsPath))
            {
                DirectoryInfo direction = new DirectoryInfo(scriptsPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    //忽略关联文件与特殊类
                    if (!files[i].Name.EndsWith(".meta") && files[i].Name == loadScriptsName + ".cs")
                    {
                        string oldScriptsContent = FileOperation.GetTextToLoad(FileOperation.ConvertToLocalPath(files[i].FullName));
                        string newScriptsContent = ReplaceScriptContent(oldScriptsContent, GetReplaceContent(), "//属性生成开始", "//属性生成结束");
                        FileOperation.SaveTextToLoad(GenerateGeneral.GetPath(loadScriptsName), newScriptsContent);

                        return;
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="scriptsContent"></param>
        /// <param name="insertContent"></param>
        /// <param name="insertStartMark"></param>
        /// <param name="insertEndMark"></param>
        /// <returns></returns>
        protected string ReplaceScriptContent(string scriptsContent, List<string> insertContent,
            string insertStartMark, string insertEndMark)
        {
            if (scriptsContent.Contains(insertStartMark) && scriptsContent.Contains(insertEndMark))
            {
                //开始位置 
                int usingStartIndex = scriptsContent.IndexOf(insertStartMark, StringComparison.Ordinal);
                //结束位置
                int usingEndIndex = scriptsContent.IndexOf(insertEndMark, StringComparison.Ordinal);
                //移除多余空格
                while (scriptsContent[usingEndIndex - 1] == ' ')
                {
                    usingEndIndex -= 1;
                }

                //查找要被替换的内容
                string scriptUsingContent = String.Empty;
                for (int i = 0; i < scriptsContent.Length; i++)
                {
                    if (i >= usingStartIndex && i < usingEndIndex)
                    {
                        scriptUsingContent += scriptsContent[i];
                    }
                }

                string tempInsertContent = String.Empty;
                for (int i = 0; i < insertContent.Count; i++)
                {
                    tempInsertContent += Indents(4) + "public" + Indents(1) + "static" + Indents(1) + "string" + Indents(1) + insertContent[i] + Indents(1) + "=" + "\"" + insertContent[i] + "\"" +
                                         ";\n";
                }

                tempInsertContent = insertStartMark + "\n" + tempInsertContent + "\n";
                //替换新内容
                return scriptsContent.Replace(scriptUsingContent, tempInsertContent);
            }
            else
            {
                return scriptsContent;
            }
        }

        protected string Indents(int number)
        {
            string temp = String.Empty;
            for (int i = 0; i < number; i++)
            {
                temp += " ";
            }

            return temp;
        }
#endif
    }
}