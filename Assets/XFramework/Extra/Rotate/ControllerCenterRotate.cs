using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class ControllerCenterRotate : SceneComponent
    {
        [BoxGroup("基础")] [LabelText("开启")] public bool playing = true;
        [BoxGroup("基础")] [LabelText("开启UI遮挡")] public bool uiOcclusion = true;
        [BoxGroup("基础")] [LabelText("目标物体")] public Transform centerTarget;
        [BoxGroup("基础")] [LabelText("旋转物体")] public Transform rotateTarget;

        [BoxGroup("基础")] [LabelText("目标偏移")] public Vector3 targetOffset;
        private float _velocity;
        [LabelText("目标距离")] private float targetDistance;
        [SerializeField] private float x = 0.0f;
        [SerializeField] private float y = 0f;

        #region 缩放

        [BoxGroup("缩放")] [LabelText("是否可缩放")] public bool canManualZoom = true;
        [BoxGroup("缩放")] [LabelText("距离")] public float distance = 100.0f;
        [BoxGroup("缩放")] [LabelText("缩放速度")] public float movSpeedScroll = 15f;
        [BoxGroup("缩放")] [LabelText("最近距离")] public float nearLimit = 0.8f;
        [BoxGroup("缩放")] [LabelText("最远距离")] public float farLimit = 4f;
        float smoothTime = 0.3f;

        [BoxGroup("缩放/触屏")] [SerializeField] [LabelText("上一次第一个触摸点位置")]
        private Vector2 oldTouch1Pos; //上次触摸点1(手指1)  

        [BoxGroup("缩放/触屏")] [SerializeField] [LabelText("上一次第二个触摸点位置")]
        private Vector2 oldTouch2Pos; //上次触摸点2(手指2) 

        #endregion

        #region 旋转

        [BoxGroup("旋转")] [LabelText("是否可旋转")] public bool canManualRotate = true;
        [BoxGroup("旋转")] [LabelText("旋转按键")] public KeyCode rotateCode = KeyCode.Mouse1;
        [BoxGroup("旋转")] [LabelText("开启水平轴向")] public bool isHorizontal = false;
        [BoxGroup("旋转")] [LabelText("开启垂直轴向")] public bool isVertical = false;
        [BoxGroup("旋转")] [LabelText("水平旋转速度")] public float xSpeed = 250.0f;
        [BoxGroup("旋转")] [LabelText("垂直旋转速度")] public float ySpeed = 120.0f;

        [BoxGroup("旋转")] [LabelText("垂直旋转最小角度")]
        public float yMinLimit = -20f;

        [BoxGroup("旋转")] [LabelText("垂直旋转最大角度")]
        public float yMaxLimit = 80f;

        [BoxGroup("旋转")] [LabelText("水平旋转最小角度")]
        public float xMinLimit = -360;

        [BoxGroup("旋转")] [LabelText("水平旋转最大角度")]
        public float xMaxLimit = 360;

        #endregion

        #region 移动

        [BoxGroup("移动")] [LabelText("是否可移动")] public bool canManualMove = true;
        [BoxGroup("移动")] [LabelText("移动反转")] public bool moveReversal = true;
        [BoxGroup("移动")] [LabelText("移动按键")] public KeyCode moveCode;
        [BoxGroup("移动")] [LabelText("移动速度")] public float movSpeed = 0.005f;

        [BoxGroup("移动")] [LabelText("本地X限定")] public Vector2 xTargetLimit;

        [BoxGroup("移动")] [LabelText("本地Y限定")] public Vector2 yTargetLimit;

        [BoxGroup("移动")] [LabelText("本地Z限定")] public Vector2 zTargetLimit;

        [BoxGroup("移动")] [LabelText("目标默认世界位置")]
        public Vector3 targetWorldDefaultPos;

        [BoxGroup("移动")] [LabelText("目标默认局部位置")]
        public Vector3 targetSelfDefaultPos;

        [BoxGroup("移动")] [LabelText("移动增量")] public Vector3 movingIncrement;
        [BoxGroup("移动")] [LabelText("X轴前限制")] public bool selfLeftDirectionLimit;
        [BoxGroup("移动")] [LabelText("X轴后限制")] public bool selfRightDirectionLimit;
        [BoxGroup("移动")] [LabelText("Y轴前限制")] public bool selfUpDirectionLimit;
        [BoxGroup("移动")] [LabelText("Y轴后限制")] public bool selfDownDirectionLimit;
        [BoxGroup("移动")] [LabelText("Z轴前限制")] public bool selfForwardDirectionLimit;
        [BoxGroup("移动")] [LabelText("Z轴后限制")] public bool selfBackDirectionLimit;
        float _yDis, _xDis;
        private bool _firstMove;
        private Vector3 _oldMousePos;
        private Vector3 _currentMousePos;
        private Vector3 _oldTargetPos;

        #endregion

        private Vector3 _startPos;

        public void Init(string entityName)
        {
            centerTarget = GetFirstEntityItemByName<Transform>(entityName);
            Init();
        }

        private void Init()
        {
            targetWorldDefaultPos = centerTarget.position;
            targetSelfDefaultPos = centerTarget.localPosition;

            distance = Vector3.Distance(centerTarget.position, rotateTarget.transform.position);
            targetDistance = Vector3.Distance(centerTarget.position, rotateTarget.transform.position);
            _startPos = DataFrameComponent.GetInspectorEuler(rotateTarget.transform);
            // _startPos = rotateTarget.transform.localEulerAngles.y;

            x = rotateTarget.transform.localEulerAngles.y;
            y = rotateTarget.transform.localEulerAngles.x;
        }

        private void FixedUpdate()
        {
            if (!playing)
            {
                return;
            }

            if (uiOcclusion)
            {
                /*if (RuntimeDataFrameComponent.Instance.uiOcclusion)
                {
                    return;
                }*/
            }

            MouseFunction();
            // Debug.Log(x);
            x = ClampAngle(x, xMinLimit, xMaxLimit);
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            
            
            if (canManualRotate)
            {
                Quaternion rotation = Quaternion.Euler(y, x, 0);

                rotateTarget.transform.rotation = Quaternion.Slerp(rotateTarget.transform.rotation, rotation, 0.2f);
                // transform.localRotation = Quaternion.Euler(x, y, 0.0f);

                // rotateTarget.transform.position = Vector3.Lerp(rotateTarget.transform.position, targetPos, Time.deltaTime * 50);
            }

            if (canManualZoom)
            {
                Vector3 targetPos = rotateTarget.transform.rotation * new Vector3(0, 0, -distance) + centerTarget.position + targetOffset;
                rotateTarget.transform.position = targetPos;
            }
        }

        public static float DampenFactor(float speed, float elapsed)
        {
            if (speed < 0.0f)
            {
                return 1.0f;
            }

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return 1.0f;
            }
#endif

            return 1.0f - Mathf.Pow((float)System.Math.E, -speed * elapsed);
        }

        /// <summary>
        /// 鼠标控制相机
        /// </summary>
        private void MouseFunction()
        {
            // Debug.Log(Input.touchCount);
            //旋转
            if (canManualRotate && !Input.GetKey(moveCode))
            {
                #region 鼠标键盘

                if (Input.GetKey(rotateCode) && Input.touchCount == 0)
                {
                    // Debug.Log("旋转");
                    // Debug.Log(Input.GetAxis("Mouse X"));

                    if (isHorizontal)
                    {
                        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f)
                        {
                            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                        }
                    }

                    if (isVertical)
                    {
                        if (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
                        {
                            y -= Input.GetAxis("Mouse Y") * xSpeed * 0.02f;
                        }
                    }
                }

                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 deltaPos = touch.deltaPosition;
                    // Debug.Log("手指操作" + deltaPos.x + ":" + deltaPos.y);
                    // Debug.Log(Input.GetAxis("Mouse X"));

                    if (isHorizontal)
                    {
                        float rotateValue = -deltaPos.x * xSpeed * 0.1f;
                        float factor = DampenFactor(1, Time.deltaTime);
                        x = Mathf.Lerp(x, x - rotateValue, factor);
                        // x -= rotateValue;
                    }

                    if (isVertical)
                    {
                        y -= deltaPos.y * xSpeed * 0.001f;
                    }
                }

                #endregion
            }

            //移动
            if (canManualMove)
            {
                //双指会触发移动操作,这里要进行屏蔽
                if ((Input.GetKey(moveCode) && Input.touchCount == 0) || Input.touchCount == 3)
                {
                    // Debug.Log("移动");
                    _currentMousePos = Input.mousePosition;
                    if (!_firstMove)
                    {
                        _oldMousePos = _currentMousePos;
                        _firstMove = true;
                        return;
                    }

                    if (_oldMousePos != _currentMousePos)
                    {
                        _yDis = (_currentMousePos.y - _oldMousePos.y) * movSpeed;
                        _xDis = (_currentMousePos.x - _oldMousePos.x) * movSpeed;
                        _oldMousePos = _currentMousePos;
                        Vector3 cameraValue = DataFrameComponent.GetInspectorEuler(rotateTarget.transform);
                        Quaternion rot = Quaternion.Euler(cameraValue.x, cameraValue.y, 0);
                        if (moveReversal)
                        {
                            _xDis = -_xDis;
                            _yDis = -_yDis;
                        }

                        //目标世界坐标
                        Vector3 targetWorldPos = rot * new Vector3(_xDis, _yDis, 0) + centerTarget.position;
                        //目标局部坐标
                        Vector3 targetSelfPos;
                        //不包含父物体
                        if (centerTarget.parent == null)
                        {
                            //世界坐标等于局部坐标
                            targetSelfPos = targetWorldPos;
                        }
                        else
                        {
                            //将世界坐标转换局部坐标
                            targetSelfPos = centerTarget.parent.InverseTransformPoint(targetWorldPos);
                        }

                        // targetSelfPos = new Vector3(-targetSelfPos.x, targetSelfPos.y, targetSelfPos.z);
                        //局部坐标增加限制
                        targetSelfPos = LimitTargetPos(targetSelfPos);
                        centerTarget.localPosition = targetSelfPos;
                        movingIncrement = targetSelfPos - _oldTargetPos;
                        _oldTargetPos = targetSelfPos;
                        targetOffset = targetSelfPos;
                    }
                    else
                    {
                        movingIncrement = Vector3.zero;
                    }
                }
                else
                {
                    //位置修正
                    // Init();
                    _oldMousePos = Input.mousePosition;
                }
            }
            else
            {
                movingIncrement = Vector3.zero;
                _firstMove = false;
                _yDis = 0;
                _xDis = 0;
                _currentMousePos = Vector3.zero;
                _oldMousePos = Vector3.zero;
            }

            //缩放
            if (canManualZoom)
            {
                //鼠标
                if (Input.touchCount == 0 && Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    targetDistance -= Input.GetAxis("Mouse ScrollWheel") * movSpeedScroll;
                    targetDistance = Mathf.Clamp(targetDistance, nearLimit, farLimit);
                }

                //触屏
                if (Input.touchCount == 2)
                {
                    //多点触摸, 放大缩小  
                    Touch newTouch1 = Input.GetTouch(0);
                    Touch newTouch2 = Input.GetTouch(1);

                    //第2点刚开始接触屏幕, 只记录，不做处理  
                    if (newTouch2.phase == TouchPhase.Began)
                    {
                        oldTouch1Pos = newTouch1.position;
                        oldTouch2Pos = newTouch2.position;
                        return;
                    }

                    //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                    float oldDistance = Vector2.Distance(oldTouch1Pos, oldTouch2Pos);
                    float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                    float offset = 0;
                    //两手指之间距离过小不进行操作
                    if (newDistance > 150)
                    {
                        offset = (newDistance - oldDistance) * movSpeedScroll * 0.01f;
                    }

                    //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                    targetDistance -= offset;
                    targetDistance = Mathf.Clamp(targetDistance, nearLimit, farLimit);
                    oldTouch1Pos = newTouch1.position;
                    oldTouch2Pos = newTouch2.position;
                }

                distance = Mathf.SmoothDamp(distance, targetDistance, ref _velocity, smoothTime);
                distance = Mathf.Clamp(distance, nearLimit, farLimit);
            }
        }

        /// <summary>
        /// 限定角度
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        /// 限制移动
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private Vector3 LimitTargetPos(Vector3 pos)
        {
            float posx = Mathf.Clamp(pos.x, xTargetLimit.x, xTargetLimit.y);
            float posy = Mathf.Clamp(pos.y, yTargetLimit.x, yTargetLimit.y);
            float posz = Mathf.Clamp(pos.z, zTargetLimit.x, zTargetLimit.y);

            return new Vector3(posx, posy, posz);
        }

        /// <summary>
        /// 开启移动
        /// </summary>
        public void Play()
        {
            playing = true;
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void Stop()
        {
            playing = false;
        }

        public override void StartComponent()
        {
        }

        public override void EndComponent()
        {
        }
    }
}