#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    [InitializeOnLoad]
    public class CustomBaseWindowHierarchy
    {
        static CustomBaseWindowHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyShow;
        }

        private static void HierarchyShow(int instanceid, Rect selectionrect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                if (obj.GetComponent<BaseWindow>() != null && obj.GetComponent<ChildBaseWindow>() == null)
                {
                    BaseWindow tempBaseWindow = obj.GetComponent<BaseWindow>();

                    #region 静态

                    if (tempBaseWindow.GetViewShowType() == ViewShowType.Static)
                    {
                        GUI.Label(SetRect(selectionrect, 0, 18), "S");
                    }

                    #endregion

                    #region 描述

                    if (!string.IsNullOrEmpty(tempBaseWindow.viewName))
                    {
                        string viewName = " --- " + tempBaseWindow.viewName;
                        Rect viewNameRect;
                        if (General.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 + DataComponent.CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = SetRect(selectionrect, -40 - ((viewName.Length - 1) * 12f), viewName.Length * 15);
                        }

                        GUI.Label(viewNameRect, viewName, new GUIStyle()
                        {
                            fontStyle = FontStyle.Italic
                        });
                    }

                    #endregion

                    Rect rectCheck = new Rect(selectionrect);
                    rectCheck.x += rectCheck.width - 20;
                    rectCheck.width = 18;
                    GameObject window = obj.transform.Find("Window").gameObject;
                    window.SetActive(GUI.Toggle(SetRect(selectionrect, -20, 18), window.activeSelf, string.Empty));
                    if (window.GetComponent<CanvasGroup>())
                    {
                        window.GetComponent<CanvasGroup>().alpha = window.activeSelf ? 1 : 0;
                    }
                    else
                    {
                        Debug.Log(window.transform.parent.name);
                    }
#if UNITY_2019_1_OR_NEWER
                    SceneVisibilityManager.instance.DisablePicking(obj, false);
                    SceneVisibilityManager.instance.DisablePicking(window, false);
#endif
                }
            }
        }

        private static Rect SetRect(Rect selectionRect, float offset, float width)
        {
            Rect rect = new Rect(selectionRect);
            rect.x += rect.width + offset;
            rect.width = width;
            return rect;
        }
    }
}
#endif