#if UNITY_EDITOR
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace XFramework
{
    public static class WindowBaseEditor
    {
        [MenuItem("GameObject/Create Empty WindowView", false, 0)]
        public static void OnCreateEmptyWindowView()
        {
            Canvas[] allCanvas = Object.FindObjectsOfType<Canvas>();

            Canvas uiCanvas = null;
            foreach (Canvas canvas in allCanvas)
            {
                if (canvas.sortingOrder == 0)
                {
                    uiCanvas = canvas;
                    break;
                }
            }

            if (uiCanvas == null)
            {
                GameObject CanvasObj = new GameObject("Canvas");
                uiCanvas = CanvasObj.AddComponent<Canvas>();
                uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler canvasScaler = CanvasObj.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920, 1080);
                CanvasObj.AddComponent<GraphicRaycaster>();
                GameObject EventSystem = new GameObject("EventSystem");
                EventSystem.AddComponent<EventSystem>();
                EventSystem.AddComponent<StandaloneInputModule>();
            }

            //View 窗口根目录
            GameObject windowView = new GameObject("Empty WindowView");
            Undo.RegisterCreatedObjectUndo(windowView, "Empty WindowView");
            Vector2 windowSize = uiCanvas.GetComponent<CanvasScaler>().referenceResolution;
            windowView.AddComponent<RectTransform>().sizeDelta = Vector2.zero;
            // windowView.AddComponent<GenerationBaseWindow>().Init();
            windowView.AddComponent<BaseWindowGenerateScripts>();
            //Window目录
            GameObject window = new GameObject("Window");
            window.AddComponent<RectTransform>().sizeDelta = windowSize;
            window.AddComponent<CanvasGroup>();
            //背景
            GameObject background = new GameObject("Background");
            background.AddComponent<Image>().rectTransform.sizeDelta = Vector2.zero;
            windowView.transform.SetParent(uiCanvas.transform);
            window.transform.SetParent(windowView.transform);
            background.transform.SetParent(window.transform);
            //Transform 调整
            windowView.transform.localPosition = Vector3.zero;
            windowView.transform.localScale = Vector3.one;
            window.transform.localPosition = Vector3.zero;
            window.transform.localScale = Vector3.one;
            background.transform.localPosition = Vector3.zero;
            background.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/生成 /@(Alt+V) 绑定UI类型  &v", false, 0)]
        public static void BindComponent()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null)
            {
                if (!uiObj.GetComponent<BindUiType>())
                {
                    Undo.AddComponent<BindUiType>(uiObj);
                }

                if (uiObj.GetComponent<Button>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Button;
                    uiObj.GetComponent<BindUiType>().eventTriggerType = General.UIEventTriggerType.PointerClick;
                }
                else if (uiObj.GetComponent<Image>() &&
                         !uiObj.GetComponent<Button>() &&
                         !uiObj.GetComponent<Scrollbar>() &&
                         !uiObj.GetComponent<ScrollRect>() &&
                         !uiObj.GetComponent<RawImage>() &&
                         // !uiObj.GetComponent<TMP_InputField>() &&
                         !uiObj.GetComponent<InputField>() &&
                         // !uiObj.GetComponent<TMP_Dropdown>() &&
                         !uiObj.GetComponent<Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Image;
                }
                else if (uiObj.GetComponent<Text>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Text;
                }
                else if (uiObj.GetComponent<Toggle>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Toggle;
                }
                else if (uiObj.GetComponent<RawImage>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.RawImage;
                }
                else if (uiObj.GetComponent<Scrollbar>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Scrollbar;
                }
                else if (uiObj.GetComponent<Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Dropdown;
                }
                else if (uiObj.GetComponent<InputField>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.InputField;
                }
                else if (uiObj.GetComponent<ScrollRect>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.ScrollRect;
                }
                else if (uiObj.GetComponent<VideoPlayer>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.VideoPlayer;
                }
                else if (uiObj.GetComponent<Slider>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.Slider;
                }
                else if (uiObj.GetComponentInChildren<ChildBaseWindow>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.ChildList;
                    uiObj.GetComponent<BindUiType>().childType = uiObj.GetComponentInChildren<ChildBaseWindow>();
                }
                else if (uiObj.GetComponentInChildren<TMP_Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.TMP_Dropdown;
                }
                else if (uiObj.GetComponentInChildren<TMP_InputField>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.TMP_InputField;
                }
                else if (uiObj.GetComponentInChildren<TextMeshProUGUI>())
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.TextMeshProUGUI;
                }
                else
                {
                    uiObj.GetComponent<BindUiType>().type = General.UiType.GameObject;
                }
            }
        }
    }
}
#endif