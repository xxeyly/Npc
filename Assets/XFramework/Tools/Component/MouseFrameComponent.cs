using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    /// <summary>
    /// 鼠标组件
    /// </summary>
    public class MouseFrameComponent : FrameComponent
    {
        public static MouseFrameComponent Instance;

        public enum SceneObjectFollowingModel
        {
            Normal,
            Select,
            MouseDown
        }

        /// <summary>
        /// 鼠标跟随
        /// </summary>
        private int _objectFollowingMouseTaskTime;

        /// <summary>
        /// 跟随模式
        /// </summary>
        private SceneObjectFollowingModel _sceneObjectFollowingModel;

        /// <summary>
        /// 场景相机
        /// </summary>
        private Camera _sceneCamera;

        /// <summary>
        /// 是否开启跟随
        /// </summary>
        private bool _isSceneFollow;

        private Vector3 _vec3TargetScreenSpace; //目标物体的屏幕空间坐标  
        private Vector3 _vec3TargetWorldSpace; // 目标物体的世界空间坐标  
        private Transform _trans; //目标物体的空间变换组件  
        private Vector3 _vec3MouseScreenSpace; // 鼠标的屏幕空间坐标  
        private Vector3 _vec3Offset; // 偏移 
#pragma warning disable 649
        [LabelText("鼠标常态")]
        [SerializeField] private Texture2D _mouseNormal; // 鼠标常态
        [LabelText("鼠标移入")]
        [SerializeField] private Texture2D _mouseEnter; // 鼠标移入
#pragma warning restore
        private Canvas _canvas;
        private GameObject _mouseEnterUi;
        [LabelText("鼠标按下")]
        public bool mouseDown;

        public override void FrameInitComponent()
        {
            Instance = GetComponent<MouseFrameComponent>();
        }
        public override void FrameSceneInitComponent()
        {
            
        }


        public override void FrameEndComponent()
        {
            Instance = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !GetMouseOverUI())
            {
                mouseDown = true;
                AudioFrameComponent.Instance.PlayEffectAudio("点击");
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
            }
        }

        /// <summary>
        /// 目标物体
        /// </summary>
        private Transform _targetObj;

        public void SetMouseArrow(Texture2D spr, Vector2 offset)
        {
            Cursor.SetCursor(spr, offset, CursorMode.Auto);
        }

        public void SetMouseArrow()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        /// <summary>
        /// 开始物品跟随相机
        /// </summary>
        public void StartObjectFollowingMouse(Camera sceneCamera, GameObject targetObject,
            SceneObjectFollowingModel sceneObjectFollowingModel)
        {
            _isSceneFollow = true;
            _sceneCamera = sceneCamera;
            _trans = targetObject.transform;
            _sceneObjectFollowingModel = sceneObjectFollowingModel;
            _targetObj = targetObject.transform;
        }

        /// <summary>
        /// 停止物品跟随相机
        /// </summary>
        public void StopObjectFollowingMouse()
        {
            _isSceneFollow = false;
            _targetObj = null;
        }

        private void FixedUpdate()
        {
            if (_sceneObjectFollowingModel == SceneObjectFollowingModel.MouseDown && _isSceneFollow)
            {
                // 把目标物体的世界空间坐标转换到它自身的屏幕空间坐标   

                var position = _trans.position;
                _vec3TargetScreenSpace = _sceneCamera.WorldToScreenPoint(position);

                // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）   

                _vec3MouseScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, _vec3TargetScreenSpace.z);

                // 计算目标物体与鼠标物体在世界空间中的偏移量   

                _vec3Offset = position - _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace);
                if (Input.GetMouseButton(0))
                {
                    // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）  

                    _vec3MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        _vec3TargetScreenSpace.z);

                    // 把鼠标的屏幕空间坐标转换到世界空间坐标（Z值使用目标物体的屏幕空间坐标），加上偏移量，以此作为目标物体的世界空间坐标  

                    _vec3TargetWorldSpace = _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace) /* + mVec3Offset*/;
                    Debug.Log(_vec3TargetWorldSpace);
                    _targetObj.position = _vec3TargetWorldSpace;
                    //}
                }
            }

            if (_sceneObjectFollowingModel == SceneObjectFollowingModel.Normal && _isSceneFollow)
            {
                // 把目标物体的世界空间坐标转换到它自身的屏幕空间坐标   

                var position = _trans.position;
                _vec3TargetScreenSpace = _sceneCamera.WorldToScreenPoint(position);

                // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）   

                _vec3MouseScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, _vec3TargetScreenSpace.z);

                // 计算目标物体与鼠标物体在世界空间中的偏移量   

                _vec3Offset = position - _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace);

                // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）  

                _vec3MouseScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, _vec3TargetScreenSpace.z);

                // 把鼠标的屏幕空间坐标转换到世界空间坐标（Z值使用目标物体的屏幕空间坐标），加上偏移量，以此作为目标物体的世界空间坐标  

                _vec3TargetWorldSpace = _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace) /* + mVec3Offset*/;
                Debug.Log(_vec3TargetWorldSpace);
                _targetObj.position = _vec3TargetWorldSpace;
                //}
            }
        }

        IEnumerator OnMouseDown()
        {
            if (_sceneObjectFollowingModel == SceneObjectFollowingModel.Select && _isSceneFollow)
            {
                // 把目标物体的世界空间坐标转换到它自身的屏幕空间坐标   

                var position = _trans.position;
                _vec3TargetScreenSpace = _sceneCamera.WorldToScreenPoint(position);

                // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）   

                _vec3MouseScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, _vec3TargetScreenSpace.z);

                // 计算目标物体与鼠标物体在世界空间中的偏移量   

                _vec3Offset = position - _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace);
                while (Input.GetMouseButton(0))
                {
                    // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）  

                    _vec3MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        _vec3TargetScreenSpace.z);

                    // 把鼠标的屏幕空间坐标转换到世界空间坐标（Z值使用目标物体的屏幕空间坐标），加上偏移量，以此作为目标物体的世界空间坐标  

                    _vec3TargetWorldSpace = _sceneCamera.ScreenToWorldPoint(_vec3MouseScreenSpace) + _vec3Offset;
                    Debug.Log(_vec3TargetWorldSpace);

                    _targetObj.position = _vec3TargetWorldSpace;
                    //}


                    // 等待固定更新   

                    yield return new WaitForFixedUpdate();
                }
            }
        }

        /// <summary>
        /// 开始物体跟随鼠标
        /// </summary>
        /// <param name="targetObj"></param>
        /// <param name="offset">偏差</param>
        public void StartUiFollowingMouse(GameObject targetObj, Vector2 offset = new Vector2())
        {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            UiFollowingMouse(targetObj, offset);
            _objectFollowingMouseTaskTime =
                TimeFrameComponent.Instance.AddTimeTask(() => { UiFollowingMouse(targetObj, offset); }, "UI拖拽任务", 0.00f, 0);
        }

        /// <summary>
        /// UI物体跟随鼠标移动
        /// </summary>
        /// <param name="targetObj"></param>
        /// <param name="offset"></param>
        private void UiFollowingMouse(GameObject targetObj, Vector2 offset = new Vector2())
        {
            Vector2 uiPos = new Vector2();

            switch (_canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                        Input.mousePosition, null, out uiPos);
                    break;
                case RenderMode.ScreenSpaceCamera:
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                        Input.mousePosition, _canvas.worldCamera, out uiPos);

                    break;
                case RenderMode.WorldSpace:
                    break;
            }

            uiPos += new Vector2(-offset.x, -offset.y);
            targetObj.GetComponent<RectTransform>().localPosition = uiPos;
            // Debug.Log("拖拽物体的位置:" + targetObj.transform.localPosition);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        public void StopUiFollowingMouse()
        {
            TimeFrameComponent.Instance.DeleteTimeTask(_objectFollowingMouseTaskTime);
        }


        public void MouseEnterState()
        {
            #region UI

            if (RuntimeDataFrameComponent.Instance.mouseState)
            {
                _mouseEnterUi = GetMouseEnterUi();
                if (_mouseEnterUi != null)
                {
                    if (_mouseEnterUi.GetComponent<Button>() || _mouseEnterUi.GetComponent<Toggle>() ||
                        _mouseEnterUi.GetComponent<Slider>())
                    {
                        SetMouseArrow(_mouseEnter, new Vector2(0, 0));
                    }
                    else if (_mouseEnterUi.GetComponent<Image>() && _mouseEnterUi.GetComponent<Image>().raycastTarget)
                    {
                        SetMouseArrow(_mouseEnter, new Vector2(0, 0));
                    }
                    else
                    {
                        // Debug.Log(_mouseEnterUI);
                    }
                }
                else
                {
                    SetMouseArrow(_mouseNormal, new Vector2(0, 0));
                }

                // Debug.Log(_mouseEnterUI);
            }

            #endregion
        }

        /// <summary>
        /// 鼠标停留在UI上
        /// </summary>
        /// <returns></returns>
        public bool GetMouseOverUI()
        {
            if (EventSystem.current != null)
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                    {position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)};
                //鼠标位置
                //一个由光线射到的物体的List
                List<RaycastResult> results = new List<RaycastResult>();
                //获取list
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results); //results.Count == 1
                //通过图形射线的检测   判断物体是不是符合要求  返回相应的Bool
                if (results.Count > 0)
                {
                    return results.Count > 0;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得鼠标点击的UI
        /// </summary>
        /// <returns></returns>
        public GameObject GetMouseEnterUi()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                {position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)};
            //鼠标位置
            //一个由光线射到的物体的List
            List<RaycastResult> results = new List<RaycastResult>();
            //获取list
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results); //results.Count == 1
            //通过图形射线的检测   判断物体是不是符合要求  返回相应的Bool
            if (results.Count > 0 /*&& results[0].gameObject.tag == "equipIcon"*/)
            {
                return results[0].gameObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 物体围绕鼠标旋转
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="target"></param>
        /// <param name="distanceGrowth"></param>
        public void TargetFollowMouseRotate(Camera camera, Transform target, bool distanceGrowth = true)
        {
            target.localEulerAngles = new Vector3(0, 0, GetTwoPointAngle(camera, target, Input.mousePosition));
            if (distanceGrowth)
            {
                target.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(Vector3.Distance(target.position, Input.mousePosition) * (1920f / Screen.width),
                        target.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        /// <summary>
        /// 获得两点之间的角度
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="origin"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public float GetTwoPointAngle(Camera camera, Transform origin, Vector3 targetPos)
        {
            Vector2 targetScreenPoint = origin.position;
            Vector2 mousePos = targetPos;
            //距离
            float length = Vector2.Distance(targetScreenPoint, mousePos);
            float angle = Mathf.Asin(Mathf.Abs(targetScreenPoint.x - mousePos.x) / length) * 180 / Mathf.PI;

            if (mousePos.x > targetScreenPoint.x)
            {
                if (mousePos.y < targetScreenPoint.y)
                {
                    if (!float.IsNaN(angle))
                    {
                        return angle - 90;
                    }
                }
                else
                {
                    if (!float.IsNaN(180 - angle))
                    {
                        return 180 - angle - 90;
                    }
                }
            }
            else
            {
                if (mousePos.y < targetScreenPoint.y)
                {
                    return -angle - 90;
                }
                else
                {
                    if (!float.IsNaN(angle - 180))
                    {
                        return angle - 180 - 90;
                    }
                }
            }

            return 0;
        }

        private int _drag3DObjectInUIPanelTimeTask;

        /// <summary>
        /// 拖动3D物体在UI面板上
        /// </summary>
        public bool Drag3DObjectInUIPanel(Camera _currentCamera, Transform _target, Vector3 centerPos,
            float leftDirection, float rightDirection, float topDirection, float downDirection,
            float frontDirection, float backDirection)
        {
            Ray ray = _currentCamera.ScreenPointToRay(Input.mousePosition * (1920f / Screen.width));
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit, LayerMask.NameToLayer("CheckItem"), 10000);
            if (isHit)
            {
                // Debug.Log(hit.collider.gameObject);
            }
            else
            {
                // Debug.Log("未找到物体");
            }

            if (isHit && _target.gameObject == hit.collider.gameObject)
            {
                Debug.Log(hit.collider.gameObject);
                TimeFrameComponent.Instance.DeleteTimeTask(_drag3DObjectInUIPanelTimeTask);
                _drag3DObjectInUIPanelTimeTask = TimeFrameComponent.Instance.AddTimeTask(() =>
                {
                    Transform cameraTransform = _currentCamera.transform;
                    Vector3 CO_Direction = _target.position - cameraTransform.position;
                    float cPlane = Vector3.Dot(CO_Direction, cameraTransform.forward);
                    Vector3 currentMousePos = _currentCamera.ScreenToWorldPoint(new Vector3(
                        Input.mousePosition.x * (1920f / Screen.width), Input.mousePosition.y * (1920f / Screen.width),
                        cPlane));

                    if (currentMousePos.x <= centerPos.x - leftDirection)
                    {
                        currentMousePos.x = centerPos.x - leftDirection;
                    }

                    if (currentMousePos.x >= centerPos.x + rightDirection)
                    {
                        currentMousePos.x = centerPos.x + rightDirection;
                    }

                    if (currentMousePos.y <= centerPos.y - downDirection)
                    {
                        currentMousePos.y = centerPos.y - downDirection;
                    }

                    if (currentMousePos.y >= centerPos.y + topDirection)
                    {
                        currentMousePos.y = centerPos.y + downDirection;
                    }

                    if (currentMousePos.z <= centerPos.z - frontDirection)
                    {
                        currentMousePos.z = centerPos.z - frontDirection;
                    }

                    if (currentMousePos.z >= centerPos.z + backDirection)
                    {
                        currentMousePos.z = centerPos.z + backDirection;
                    }

                    _target.position = currentMousePos;
                }, "拖动3D物体在UI面板上", 0.001f, 0);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 停止拖动3D物体在UI面板上
        /// </summary>
        public void StopDrag3DObjectInUIPanel()
        {
            TimeFrameComponent.Instance.DeleteTimeTask(_drag3DObjectInUIPanelTimeTask);
        }
    }
}