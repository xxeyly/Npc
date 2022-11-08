using System;
using UnityEngine;

namespace XFramework
{
    public class ChildBaseWindowGenerateScripts : ViewGenerateScripts
    {
        protected override string GetScriptsPath()
        {
            Type scriptType = GetComponent<ChildBaseWindow>().GetType();
            string[] scriptNameSplit = scriptType.ToString().Split(new char[] {'.'});
            string scriptName = scriptNameSplit[scriptNameSplit.Length - 1];
            Debug.Log(scriptName);
            string scriptPath = GetPath(scriptName);
            return scriptPath;
        }

        protected override string CustomReplaceScriptContent(string currentScriptsContent)
        {
            return currentScriptsContent;
        }

        protected override Transform GetWindow()
        {
            return transform;
        }
    }
}