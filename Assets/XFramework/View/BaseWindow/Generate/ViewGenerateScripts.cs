using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract class ViewGenerateScripts : MonoBehaviour
    {
        [TabGroup("UI", "引用")] [LabelText("UI变量引用")] [ReadOnly]
        public List<string> allUiVariableUsing;

        [TabGroup("UI", "名称")] [LabelText("UI变量名称")] [ReadOnly]
        public List<string> allUiVariableName;

        [TabGroup("UI", "查找")] [LabelText("UI变量绑定")] [ReadOnly]
        public List<string> allUiVariableBind;

        [TabGroup("UI", "绑定")] [LabelText("UI变量绑定事件")] [ReadOnly]
        public List<string> allUiVariableBindListener;

        [TabGroup("UI", "事件")] [LabelText("UI变量绑定事件声明")] [ReadOnly]
        public List<string> allUiVariableBindListenerEvent;
#pragma warning disable 0649
        protected GenerateBaseWindowData GenerateBaseWindowData;
        private string _currentScriptsContent;
        [LabelText("分号")] protected string Semicolon = ";";
#pragma warning disable 0414
        [LabelText("换行")] protected string LineFeed = "\n";
#pragma warning restore 0414
        [LabelText("监听事件列表")] private Dictionary<string, string> _listenerActionList;
#pragma warning restore 0649

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void Generate()
        {
#if UNITY_EDITOR
            _currentScriptsContent = GetOldScriptsContent();

            if (GenerateBaseWindowData == null)
            {
                GenerateBaseWindowData =
                    UnityEditor.AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(
                        General.generateBaseWindowPath);
            }

            _listenerActionList = new Dictionary<string, string>();
            GenerateUsing();
            GenerateUi();
            GenerateBindUi();
            GenerateBindListener();
            GenerateOldAction();
            GenerateStatementListener();
            GenerateCustomData();
            for (int i = 0; i < allUiVariableUsing.Count - 1; i++)
            {
                allUiVariableUsing[i] += LineFeed;
            }

            for (int i = 0; i < allUiVariableName.Count - 1; i++)
            {
                allUiVariableName[i] += LineFeed;
            }

            for (int i = 0; i < allUiVariableBind.Count - 1; i++)
            {
                allUiVariableBind[i] += LineFeed;
            }

            for (int i = 0; i < allUiVariableBindListener.Count - 1; i++)
            {
                allUiVariableBindListener[i] += LineFeed;
            }

            for (int i = 0; i < allUiVariableBindListenerEvent.Count - 1; i++)
            {
                allUiVariableBindListenerEvent[i] += LineFeed;
            }

            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableUsing,
               GenerateBaseWindowData.startUsing,
                GenerateBaseWindowData.endUsing);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableName,
               GenerateBaseWindowData.startUiVariable,
                 GenerateBaseWindowData.endUiVariable);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBind,
                GenerateBaseWindowData.startVariableBindPath,
                GenerateBaseWindowData.endVariableBindPath);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBindListener,
                GenerateBaseWindowData.startVariableBindListener,
                GenerateBaseWindowData.endVariableBindListener);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBindListenerEvent,
                GenerateBaseWindowData.startVariableBindEvent,
                 GenerateBaseWindowData.endVariableBindEvent);
            _currentScriptsContent = CustomReplaceScriptContent(_currentScriptsContent);
            FileOperation.SaveTextToLoad(GetScriptsPath(), _currentScriptsContent);
            ClearConsole();
#endif
        }

#if UNITY_EDITOR
#pragma warning disable 0414
        static MethodInfo _clearMethod;
#pragma warning restore 0414
        /// <summary>
        /// 清空log信息
        /// </summary>
        private static void ClearConsole()
        {
            if (_clearMethod == null)
            {
                Type log = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
                _clearMethod = log.GetMethod("Clear");
            }

            if (_clearMethod != null) _clearMethod.Invoke(null, null);
            Debug.Log("代码生成完毕");
        }

#endif

        /// <summary>
        /// 缩进
        /// </summary>
        /// <returns></returns>
        protected string Indents(int number)
        {
            string temp = String.Empty;
            for (int i = 0; i < number; i++)
            {
                temp += " ";
            }

            return temp;
        }

        /// <summary>
        /// 获得Window界面 LocalBaseWindow从跟目录开始索引
        /// </summary>
        /// <returns></returns>
        protected virtual Transform GetWindow()
        {
            return transform.Find("Window").transform;
        }

        /// <summary>
        /// 获得UI组件是否包含LocalBaseWindow路径
        /// </summary>
        /// <returns></returns>
        protected bool GetUiComponentContainLocalBaseWindow(Transform uiTr)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            if (hierarchy == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i <= hierarchy; i++)
                {
                    if (GetParentByHierarchy(defaultUiTr, i).GetComponent<ChildBaseWindow>())
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// 根据UI层级获得父物体
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        private static Transform GetParentByHierarchy(Transform uiTr, int hierarchy)
        {
            for (int i = 0; i < hierarchy; i++)
            {
                uiTr = uiTr.parent;
            }

            return uiTr;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            for (int i = 1; i <= hierarchy; i++)
            {
                uiTr = GetParentByHierarchy(defaultUiTr, i);
                uiPath = uiTr.name + "/" + uiPath;
            }

            return uiPath + defaultUiTr.name;
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
                    tempInsertContent += insertContent[i];
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

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetScriptsPath()
        {
            Type scriptType = GetComponent<BaseWindow>().GetType();
            string[] scriptNameSplit = scriptType.ToString().Split(new char[] { '.' });
            string scriptName = scriptNameSplit[scriptNameSplit.Length - 1];
            string scriptPath = GetPath(scriptName);
            return scriptPath;
        }

        /// <summary>
        /// 获得旧的脚本内容
        /// </summary>
        private string GetOldScriptsContent()
        {
            string oldScriptContent = FileOperation.GetTextToLoad(GetScriptsPath());
            return oldScriptContent;
        }

        /// <summary>
        /// 添加Using引用
        /// </summary>
        /// <param name="usingContent"></param>
        protected void AddUsing(string usingContent)
        {
            //using列表不包含,并且旧代码不包含
            if (!allUiVariableUsing.Contains(usingContent) && !_currentScriptsContent.Contains(usingContent))
            {
                allUiVariableUsing.Add(usingContent);
            }
        }

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        protected static string GetPath(string scriptName)
        {
#if UNITY_EDITOR

            string[] path = UnityEditor.AssetDatabase.FindAssets(scriptName);

            for (int i = 0; i < path.Length; i++)
            {
                Debug.Log(UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]));
                if (UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]).Contains("Assets") &&
                    Path.GetFileName(UnityEditor.AssetDatabase.GUIDToAssetPath(path[i])) == (scriptName + ".cs"))
                {
                    return UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]);
                }
            }

#endif
            return null;
        }

        /// <summary>
        /// 生成Using
        /// </summary>
        public void GenerateUsing()
        {
            string insertStartMark = GenerateBaseWindowData.startUsing;
            string insertEndMark = GenerateBaseWindowData.endUsing;

            string currentScript = GetOldScriptsContent();
            //开始位置 
            int usingStartIndex =
                currentScript.IndexOf(insertStartMark, StringComparison.Ordinal) + insertStartMark.Length;
            //结束位置
            int usingEndIndex = currentScript.IndexOf(insertEndMark, StringComparison.Ordinal);
            //移除多余空格
            while (currentScript[usingEndIndex - 1] == ' ')
            {
                usingEndIndex -= 1;
            }

            //查找要被替换的内容
            string scriptContent = String.Empty;
            for (int i = 0; i < currentScript.Length; i++)
            {
                if (i >= usingStartIndex && i < usingEndIndex)
                {
                    scriptContent += currentScript[i];
                }
            }

            allUiVariableUsing = new List<string>();
            GetOldUsingContent(scriptContent);
        }

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void GenerateUi()
        {
            Transform window = GetWindow();
            allUiVariableName = new List<string>();
            //首行缩进
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                BindUiType bindUiType = child.GetComponent<BindUiType>();
                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (bindUiType.type)
                    {
                        case General.UiType.GameObject:

                            allUiVariableName.Add(Indents(4) + "private GameObject _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) + Semicolon);
                            AddUsing("using UnityEngine;");
                            break;
                        case General.UiType.Button:

                            allUiVariableName.Add(
                                Indents(4) + "private Button _" + DataFrameComponent.FirstCharToLower(child.name) + Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Image:
                            allUiVariableName.Add(Indents(4) + "private Image _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Text:
                            allUiVariableName.Add(Indents(4) + "private Text _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon);
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Toggle:
                            allUiVariableName.Add(
                                Indents(4) + "private Toggle _" + DataFrameComponent.FirstCharToLower(child.name) +
                                Semicolon);
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.RawImage:
                            allUiVariableName.Add(Indents(4) + "private RawImage _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Scrollbar:
                            allUiVariableName.Add(Indents(4) + "private Scrollbar _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.ScrollRect:
                            allUiVariableName.Add(Indents(4) + "private ScrollRect _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) + Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.InputField:

                            allUiVariableName.Add(Indents(4) + "private InputField _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) + Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Dropdown:
                            allUiVariableName.Add(Indents(4) + "private Dropdown _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.Slider:
                            allUiVariableName.Add(Indents(4) + "private Slider _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.VideoPlayer:
                            allUiVariableName.Add(Indents(4) + "private VideoPlayer _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case General.UiType.TextMeshProUGUI:
                            allUiVariableName.Add(Indents(4) + "private TextMeshProUGUI _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using TMPro;");
                            break;
                        case General.UiType.TMP_Dropdown:
                            allUiVariableName.Add(Indents(4) + "private TMP_Dropdown _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using TMPro;");
                            break;
                        case General.UiType.TMP_InputField:
                            allUiVariableName.Add(Indents(4) + "private TMP_InputField _" +
                                                  DataFrameComponent.FirstCharToLower(child.name) +
                                                  Semicolon
                            );
                            AddUsing("using TMPro;");
                            break;

                        case General.UiType.ChildList:

                            string childTypeName;
                            Type childType;
                            if (bindUiType.childType != null)
                            {
                                childType = bindUiType.childType.GetType();
                                if (childType == typeof(ChildUiBaseWindow))
                                {
                                    childTypeName = child.GetComponentInChildren<ChildUiBaseWindow>().uiType.ToString();
                                    AddUsing("using System.Collections.Generic;");
                                }
                                else
                                {
                                    childTypeName = bindUiType.childType.GetType().ToString();
                                    AddUsing("using System.Collections.Generic;");
                                }

                                childTypeName = childTypeName.Split('.')[childTypeName.Split('.').Length - 1];

                                allUiVariableName.Add(Indents(4) + "private List<" + childTypeName + "> _" +
                                                      DataFrameComponent.FirstCharToLower(child.name) + Semicolon);
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        public void GenerateBindUi()
        {
            Transform window = GetWindow();
            allUiVariableBind = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    allUiVariableBind.Add(Indents(8) + "BindUi(ref _" + DataFrameComponent.FirstCharToLower(child.name) +
                                          ",\"" +
                                          GetUiComponentPath(child, "") + "\");");
                }

                if (child.GetComponent<BindUiType>() &&
                    child.GetComponent<BindUiType>().type == General.UiType.ChildList)
                {
                    string listChildBaseWindowContent = String.Empty;
                    listChildBaseWindowContent +=
                        Indents(8) + "for" + Indents(1) + "(" + "int" + Indents(1) + "i" + Indents(1) + "=" +
                        Indents(1) + "0" + Semicolon +
                        Indents(1) + "i" + Indents(1) + "<" + Indents(1) + "_" +
                        DataFrameComponent.FirstCharToLower(child.name) + ".Count" + Semicolon + Indents(1) + "i++" +
                        ")" + LineFeed + Indents(8) + "{"
                        + LineFeed + Indents(12) + "_" + DataFrameComponent.FirstCharToLower(child.name) + "[i]" + "." +
                        "ViewStartInit();"
                        + LineFeed + Indents(12) + "_" + DataFrameComponent.FirstCharToLower(child.name) + "[i]" + "." +
                        "InitData(i);" + LineFeed + Indents(8) + "}";

                    allUiVariableBind.Add(listChildBaseWindowContent);
                }
            }
        }

        /// <summary>
        /// 一键绑定UI事件
        /// </summary>
        /// <returns></returns>
        public void GenerateBindListener()
        {
            Transform window = GetWindow();
            allUiVariableBindListener = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                BindUiType bindUiType = child.GetComponent<BindUiType>();

                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    string bindStr;
                    if (bindUiType.type == General.UiType.Button)
                    {
                        General.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;

                        if ((General.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerClick)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerClick" + "," + "On" + child.name + "Click" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerEnter)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerEnter" + "," + "On" + child.name + "Enter" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerExit)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerExit" + "," + "On" + child.name + "Exit" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerDown)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerDown" + "," + "On" + child.name + "Down" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerUp)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerUp" + "," + "On" + child.name + "Up" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            General.UIEventTriggerType.Drag)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.Drag" + "," + "On" + child.name + "Drag" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.BeginDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.BeginDrag)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.BeginDrag" + "," + "On" + child.name + "BeginDrag" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.EndDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.EndDrag)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.EndDrag" + "," + "On" + child.name + "EndDrag" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.Scroll & uiEventTriggerType) ==
                            General.UIEventTriggerType.Scroll)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataFrameComponent.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.Scroll" + "," + "On" + child.name + "Scroll" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        AddUsing("using UnityEngine.EventSystems;");
                    }
                    else if (bindUiType.type == General.UiType.Toggle)
                    {
                        bindStr = Indents(8) + "_" + DataFrameComponent.FirstCharToLower(child.name) +
                                  ".onValueChanged.AddListener(" +
                                  "On" +
                                  child.name + ");";
                        allUiVariableBindListener.Add(bindStr);
                        AddUsing("using UnityEngine.EventSystems;");
                    }
                }
            }
        }

        /// <summary>
        /// 获得旧的事件
        /// </summary>
        public void GenerateOldAction()
        {
            string insertStartMark = GenerateBaseWindowData.startVariableBindEvent;
            string insertEndMark = GenerateBaseWindowData.endVariableBindEvent;

            string currentScript = GetOldScriptsContent();
            //开始位置 
            int usingStartIndex =
                currentScript.IndexOf(insertStartMark, StringComparison.Ordinal) - insertStartMark.Length;
            //结束位置
            int usingEndIndex = currentScript.IndexOf(insertEndMark, StringComparison.Ordinal);
            //移除多余空格
            while (currentScript[usingEndIndex - 1] == ' ')
            {
                usingEndIndex -= 1;
            }

            //查找要被替换的内容
            string scriptContent = String.Empty;
            for (int i = 0; i < currentScript.Length; i++)
            {
                if (i >= usingStartIndex && i < usingEndIndex)
                {
                    scriptContent += currentScript[i];
                }
            }

            List<string> actionNameList = new List<string>();

            Transform window = GetWindow();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                BindUiType bindUiType = child.GetComponent<BindUiType>();

                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (bindUiType.type == General.UiType.Button)
                    {
                        General.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;

                        if ((General.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerClick)
                        {
                            actionNameList.Add(FindActionNameKey(
                                "On" + child.name + "Click", scriptContent));
                        }

                        if ((General.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerEnter)
                        {
                            actionNameList.Add(FindActionNameKey(
                                "On" + child.name + "Enter", scriptContent));
                        }

                        if ((General.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerExit)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Exit",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerDown)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Down",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerUp)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Up",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            General.UIEventTriggerType.Drag)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Drag",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.BeginDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.BeginDrag)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "BeginDrag",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.EndDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.EndDrag)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "EndDrag",
                                    scriptContent));
                        }

                        if ((General.UIEventTriggerType.Scroll & uiEventTriggerType) ==
                            General.UIEventTriggerType.Scroll)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Scroll",
                                    scriptContent));
                        }
                    }
                    else if (bindUiType.type == General.UiType.Toggle)
                    {
                        actionNameList.Add(FindActionNameKey("On" + child.name,
                            scriptContent));
                    }
                }
            }

            foreach (string actionName in actionNameList)
            {
                if (scriptContent.Contains(actionName))
                {
                    if (_listenerActionList.ContainsKey(actionName))
                    {
                        _listenerActionList[actionName] = FindActionContent(actionName, scriptContent);
                    }
                    else
                    {
                        _listenerActionList.Add(actionName, FindActionContent(actionName, scriptContent));
                    }
                }
            }
        }

        private void GetOldUsingContent(string usingContent)
        {
            List<string> oldUsing = new List<string>(usingContent.Split(new[] { LineFeed }, StringSplitOptions.None));
            for (int i = 0; i < oldUsing.Count; i++)
            {
                if (oldUsing[i].Contains(Semicolon))
                {
                    allUiVariableUsing.Add(oldUsing[i].Replace(LineFeed, ""));
                }
            }
        }

        /// <summary>
        /// 查找方法关键Key
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="scriptsContent"></param>
        /// <returns></returns>
        private string FindActionNameKey(string actionName, string scriptsContent)
        {
            string actionContent = "";
            //开始位置 
            int startIndex =
                scriptsContent.IndexOf(actionName, StringComparison.Ordinal) + actionName.Length;
            int endIndex = 0;

            for (int i = startIndex; i < scriptsContent.Length; i++)
            {
                if (scriptsContent[i] != '(')
                {
                    endIndex += 1;
                    break;
                }
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                actionContent += scriptsContent[startIndex + i];
            }

            return actionName + actionContent;
        }

        /// <summary>
        /// 查找方法的内容
        /// </summary>
        /// <param name="startMark"></param>
        /// <param name="scriptContent"></param>
        /// <returns></returns>
        private string FindActionContent(string startMark, string scriptContent)
        {
            string action = String.Empty;
            //开始位置 
            int startIndex =
                scriptContent.IndexOf(startMark, StringComparison.Ordinal) + startMark.Length;
            for (int i = startIndex; i < scriptContent.Length; i++)
            {
                startIndex += 1;
                if (scriptContent[i] == '{')
                {
                    break;
                }
            }

            int endIndex = 0;
            int leftBrackets = 0;
            for (int i = startIndex; i < scriptContent.Length; i++)
            {
                if (scriptContent[i] == '{')
                {
                    leftBrackets += 1;
                }

                if (scriptContent[i] == '}')
                {
                    if (leftBrackets == 0)
                    {
                        endIndex = i;
                        break;
                    }

                    leftBrackets -= 1;
                }
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                action += scriptContent[i];
            }

            return action;
        }

        /// <summary>
        /// 查找旧的函数内容
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private string FindOldActionContent(string actionName)
        {
            if (_listenerActionList.ContainsKey(actionName))
            {
                return _listenerActionList[actionName];
            }

            return "\n" + Indents(4);
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        private void GenerateStatementListener()
        {
            Transform window = GetWindow();

            allUiVariableBindListenerEvent = new List<string>();

            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == General.UiType.Button)
                    {
                        string bindStr;
                        General.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;
                        if ((General.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerClick)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Click");
                            bindStr = Indents(4) + "private void On" + child.name + "Click" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerEnter)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Enter");

                            bindStr = Indents(4) + "private void On" + child.name + "Enter" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerExit)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Exit");
                            bindStr = Indents(4) + "private void On" + child.name + "Exit" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerDown)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Down");
                            bindStr = Indents(4) + "private void On" + child.name + "Down" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            General.UIEventTriggerType.PointerUp)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Up");
                            bindStr = Indents(4) + "private void On" + child.name + "Up" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            General.UIEventTriggerType.Drag)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Drag");
                            bindStr = Indents(4) + "private void On" + child.name + "Drag" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.BeginDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.BeginDrag)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "BeginDrag");
                            bindStr = Indents(4) + "private void On" + child.name + "BeginDrag" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.EndDrag & uiEventTriggerType) ==
                            General.UIEventTriggerType.EndDrag)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "EndDrag");
                            bindStr = Indents(4) + "private void On" + child.name + "EndDrag" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((General.UIEventTriggerType.Scroll & uiEventTriggerType) ==
                            General.UIEventTriggerType.Scroll)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Scroll");
                            bindStr = Indents(4) + "private void On" + child.name + "Scroll" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }
                    }
                    else if (child.GetComponent<BindUiType>().type == General.UiType.Toggle)
                    {
                        string oldContent = FindOldActionContent(
                            "On" + child.name);
                        allUiVariableBindListenerEvent.Add(Indents(4) + "private void On" + child.name +
                                                           "(bool isOn)" + "\n" + Indents(4) + "{" + oldContent + "}");
                    }
                }
            }
        }

        protected virtual void GenerateCustomData()
        {
        }

        protected abstract string CustomReplaceScriptContent(string currentScriptsContent);
    }
}