using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XFramework
{
    public class RayRenderTools : SceneComponent
    {
        public static RayRenderTools Instance;
        [LabelText("射线相机")] public Camera currentRayCamera;
        [LabelText("双击触发间隔时间")] public float doubleClickTime;
        [LabelText("触发事件")] public List<RayRenderItemInfo> rayRenderItemInfos;
        private List<int> _tidList;

        /// <summary>
        /// 三键按下持续触发任务
        /// </summary>
        private bool _leftDownContinuousTrigger;

        private bool _rightDownContinuousTrigger;
        private bool _centerDownContinuousTrigger;

        /// <summary>
        /// 三键双击触发
        /// </summary>
        private List<GameObject> _leftDoubleClickRay;

        private List<GameObject> _rightDoubleClickRay;
        private List<GameObject> _centerDoubleClickRay;

        [Serializable]
        public class RayRenderItemInfo
        {
            [LabelText("事件ID")] public int actionId;
            [LabelText("触发事件")] public Action<RaycastHit> rayHit;
            [LabelText("离开触发事件")] public Action<RaycastHit> unRayHit;
            [LabelText("触发层")] public LayerMask currentOperationMask;
            [LabelText("鼠标类型")] public MouseType mouseType;
            [LabelText("触发类型")] public TriggerType triggerType;
            [LabelText("是否持续触发")] public bool continuousTrigger;
        }

        /// <summary>
        /// 增加射线监听
        /// </summary>
        /// <param name="rayHit"></param>
        /// <param name="currentOperationMask"></param>
        /// <param name="mouseType"></param>
        /// <param name="triggerType"></param>
        /// <param name="continuousTrigger"></param>
        /// <returns></returns>
        public int AddRayListener(Camera camera, Action<RaycastHit> rayHit, Action<RaycastHit> unRayHit, LayerMask currentOperationMask, MouseType mouseType, TriggerType triggerType,
            bool continuousTrigger = false)
        {
            currentRayCamera = camera;
            bool isCon = false;
            foreach (RayRenderItemInfo rayRenderItemInfo in rayRenderItemInfos)
            {
                if (
                    rayRenderItemInfo.currentOperationMask == currentOperationMask &&
                    rayRenderItemInfo.mouseType == mouseType &&
                    rayRenderItemInfo.triggerType == triggerType &&
                    rayRenderItemInfo.continuousTrigger == continuousTrigger)
                {
                    isCon = true;
                    break;
                }
            }

            int id = GetSwitchTaskTid();
            if (!isCon)
            {
                RayRenderItemInfo tempRenderItemInfo = new RayRenderItemInfo();
                tempRenderItemInfo.actionId = id;
                tempRenderItemInfo.rayHit = rayHit;
                tempRenderItemInfo.unRayHit = unRayHit;
                tempRenderItemInfo.currentOperationMask = currentOperationMask;
                tempRenderItemInfo.mouseType = mouseType;
                tempRenderItemInfo.triggerType = triggerType;
                tempRenderItemInfo.continuousTrigger = continuousTrigger;
                rayRenderItemInfos.Add(tempRenderItemInfo);
            }

            return id;
        }

        /// <summary>
        /// 移除射线监听
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRayListener(int id)
        {
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].actionId == id)
                {
                    rayRenderItemInfos.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveAllRayListener()
        {
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                rayRenderItemInfos.RemoveAt(i);
            }
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetSwitchTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidList.Contains(tid))
            {
                _tidList.Add(tid);
                return tid;
            }
            else
            {
                return GetSwitchTaskTid();
            }
        }


        public enum MouseType
        {
            Left = 0,
            Center = 2,
            Right = 1,
            Normal = 3
        }

        /// <summary>
        /// 触发类型
        /// </summary>
        public enum TriggerType
        {
            Down,
            Double,
            Up,
            Normal
        }


        public override void StartComponent()
        {
            Instance = GetComponent<RayRenderTools>();
            _tidList = new List<int>();
            _leftDoubleClickRay = new List<GameObject>();
            _rightDoubleClickRay = new List<GameObject>();
            _centerDoubleClickRay = new List<GameObject>();
        }

        private void Update()
        {
            UpdateRayAction();
            if (_leftDownContinuousTrigger)
            {
                List<RayRenderItemInfo> leftDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Left);
                foreach (RayRenderItemInfo rayRenderItemInfo in leftDownContinuousTriggerRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }
            }

            if (_rightDownContinuousTrigger)
            {
                List<RayRenderItemInfo> rightDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Right);
                foreach (RayRenderItemInfo rayRenderItemInfo in rightDownContinuousTriggerRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }
            }

            if (_centerDownContinuousTrigger)
            {
                List<RayRenderItemInfo> centerDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Center);
                foreach (RayRenderItemInfo rayRenderItemInfo in centerDownContinuousTriggerRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }
            }
        }


        public override void InitComponent()
        {
        }

        public override void EndComponent()
        {
        }


        /// <summary>
        /// 执行射线事件
        /// </summary>
        private void UpdateRayAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                #region 单击触发

                List<RayRenderItemInfo> leftDownRayAction = DownRayAction(MouseType.Left);
                foreach (RayRenderItemInfo rayRenderItemInfo in leftDownRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                #region 持续触发

                List<RayRenderItemInfo> leftDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Left);
                _leftDownContinuousTrigger = true;

                #endregion

                #region 双击触发

                List<RayRenderItemInfo> leftDoubleRayAction = DoubleRayAction(MouseType.Left);
                foreach (RayRenderItemInfo rayRenderItemInfo in leftDoubleRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion
            }

            if (Input.GetMouseButtonDown(1))
            {
                #region 单击触发

                List<RayRenderItemInfo> rightClickRayAction = DownRayAction(MouseType.Right);
                foreach (RayRenderItemInfo rayRenderItemInfo in rightClickRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                #region 持续触发

                List<RayRenderItemInfo> rightDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Right);
                _rightDownContinuousTrigger = true;

                #endregion

                #region 双击触发

                List<RayRenderItemInfo> rightDoubleRayAction = DoubleRayAction(MouseType.Right);
                foreach (RayRenderItemInfo rayRenderItemInfo in rightDoubleRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion
            }

            if (Input.GetMouseButtonDown(2))
            {
                #region 单击触发

                List<RayRenderItemInfo> centerClickRayAction = DownRayAction(MouseType.Center);
                foreach (RayRenderItemInfo rayRenderItemInfo in centerClickRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                #region 持续触发

                List<RayRenderItemInfo> centerDownContinuousTriggerRayAction =
                    DownContinuousTriggerRayAction(MouseType.Center);
                _centerDownContinuousTrigger = true;

                #endregion

                #region 双击触发

                List<RayRenderItemInfo> centerDoubleRayAction = DoubleRayAction(MouseType.Center);
                foreach (RayRenderItemInfo rayRenderItemInfo in centerDoubleRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion
            }

            if (Input.GetMouseButtonUp(0))
            {
                #region 单击触发

                List<RayRenderItemInfo> leftDownRayAction = UpRayAction(MouseType.Left);
                foreach (RayRenderItemInfo rayRenderItemInfo in leftDownRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                _leftDownContinuousTrigger = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                #region 单击触发

                List<RayRenderItemInfo> rightClickRayAction = UpRayAction(MouseType.Right);
                foreach (RayRenderItemInfo rayRenderItemInfo in rightClickRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                _rightDownContinuousTrigger = false;
            }

            if (Input.GetMouseButtonUp(2))
            {
                #region 单击触发

                List<RayRenderItemInfo> centerClickRayAction = UpRayAction(MouseType.Center);
                foreach (RayRenderItemInfo rayRenderItemInfo in centerClickRayAction)
                {
                    SendRay(rayRenderItemInfo);
                }

                #endregion

                _centerDownContinuousTrigger = false;
            }

            #region 其他触发

            List<RayRenderItemInfo> normalRayAction = NormalRayAction();
            foreach (RayRenderItemInfo rayRenderItemInfo in normalRayAction)
            {
                SendRay(rayRenderItemInfo);
            }

            #endregion
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <returns></returns>
        private List<RayRenderItemInfo> DoubleRayAction(MouseType mouseType)
        {
            List<RayRenderItemInfo> tempRayRenderItemInfo = new List<RayRenderItemInfo>();

            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].triggerType == TriggerType.Double)
                {
                    tempRayRenderItemInfo.Add(rayRenderItemInfos[i]);
                }
            }

            List<RayRenderItemInfo> leftTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> rightTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> centerTempRayRenderItemInfo = new List<RayRenderItemInfo>();

            switch (mouseType)
            {
                case MouseType.Left:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Left)
                        {
                            leftTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return leftTempRayRenderItemInfo;

                case MouseType.Center:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Center)
                        {
                            centerTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return centerTempRayRenderItemInfo;

                case MouseType.Right:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Right)
                        {
                            rightTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return rightTempRayRenderItemInfo;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <returns></returns>
        private List<RayRenderItemInfo> DownRayAction(MouseType mouseType)
        {
            List<RayRenderItemInfo> tempRayRenderItemInfo = new List<RayRenderItemInfo>();
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].triggerType == TriggerType.Down &&
                    rayRenderItemInfos[i].continuousTrigger == false)
                {
                    tempRayRenderItemInfo.Add(rayRenderItemInfos[i]);
                }
            }

            List<RayRenderItemInfo> leftTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> rightTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> centerTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            switch (mouseType)
            {
                case MouseType.Left:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Left)
                        {
                            leftTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return leftTempRayRenderItemInfo;

                case MouseType.Center:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Center)
                        {
                            centerTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return centerTempRayRenderItemInfo;

                case MouseType.Right:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Right)
                        {
                            rightTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return rightTempRayRenderItemInfo;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 其他
        /// </summary>
        /// <returns></returns>
        private List<RayRenderItemInfo> NormalRayAction()
        {
            List<RayRenderItemInfo> tempRayRenderItemInfo = new List<RayRenderItemInfo>();
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].triggerType == TriggerType.Normal &&
                    rayRenderItemInfos[i].continuousTrigger)
                {
                    tempRayRenderItemInfo.Add(rayRenderItemInfos[i]);
                }
            }

            return tempRayRenderItemInfo;
        }

        /// <summary>
        /// 按下持续
        /// </summary>
        /// <returns></returns>
        private List<RayRenderItemInfo> DownContinuousTriggerRayAction(MouseType mouseType)
        {
            List<RayRenderItemInfo> tempRayRenderItemInfo = new List<RayRenderItemInfo>();
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].triggerType == TriggerType.Down &&
                    rayRenderItemInfos[i].continuousTrigger)
                {
                    tempRayRenderItemInfo.Add(rayRenderItemInfos[i]);
                }
            }

            List<RayRenderItemInfo> leftTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> rightTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> centerTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            switch (mouseType)
            {
                case MouseType.Left:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Left)
                        {
                            leftTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return leftTempRayRenderItemInfo;

                case MouseType.Center:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Center)
                        {
                            centerTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return centerTempRayRenderItemInfo;

                case MouseType.Right:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Right)
                        {
                            rightTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return rightTempRayRenderItemInfo;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 抬起
        /// </summary>
        /// <returns></returns>
        private List<RayRenderItemInfo> UpRayAction(MouseType mouseType)
        {
            List<RayRenderItemInfo> tempRayRenderItemInfo = new List<RayRenderItemInfo>();
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].triggerType == TriggerType.Up)
                {
                    tempRayRenderItemInfo.Add(rayRenderItemInfos[i]);
                }
            }

            List<RayRenderItemInfo> leftTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> rightTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            List<RayRenderItemInfo> centerTempRayRenderItemInfo = new List<RayRenderItemInfo>();
            switch (mouseType)
            {
                case MouseType.Left:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Left)
                        {
                            leftTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return leftTempRayRenderItemInfo;

                case MouseType.Center:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Center)
                        {
                            centerTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return centerTempRayRenderItemInfo;

                case MouseType.Right:
                    foreach (RayRenderItemInfo rayRenderItemInfo in tempRayRenderItemInfo)
                    {
                        if (rayRenderItemInfo.mouseType == MouseType.Right)
                        {
                            rightTempRayRenderItemInfo.Add(rayRenderItemInfo);
                        }
                    }

                    return rightTempRayRenderItemInfo;
                default:
                    return null;
            }
        }


        private void SendRay(RayRenderItemInfo rayRenderItemInfo)
        {
            SendRay(rayRenderItemInfo.rayHit, rayRenderItemInfo.unRayHit, rayRenderItemInfo.currentOperationMask,
                rayRenderItemInfo.mouseType,
                rayRenderItemInfo.triggerType, rayRenderItemInfo.actionId);
        }

        private void SendRay(Action<RaycastHit> triggerAction, Action<RaycastHit> unTriggerAction,
            LayerMask currentOperationMask, MouseType mouseType,
            TriggerType triggerType, int rayTask)
        {
            Ray ray = currentRayCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100000, currentOperationMask))
            {
                if (triggerType == TriggerType.Double)
                {
                    switch (mouseType)
                    {
                        case MouseType.Left:
                            if (_leftDoubleClickRay.Contains(hit.collider.gameObject))
                            {
                                if (triggerAction == null)
                                {
                                    return;
                                }

                                triggerAction.Invoke(hit);
                                _leftDoubleClickRay.Remove(hit.collider.gameObject);
                            }
                            else
                            {
                                _leftDoubleClickRay.Add(hit.collider.gameObject);
                                TimeFrameComponent.Instance.AddTimeTask(() =>
                                {
                                    if (_leftDoubleClickRay.Contains(hit.collider.gameObject))
                                    {
                                        _leftDoubleClickRay.Remove(hit.collider.gameObject);
                                    }
                                }, "左键双击移除", doubleClickTime);
                            }

                            break;
                        case MouseType.Center:
                            if (_centerDoubleClickRay.Contains(hit.collider.gameObject))
                            {
                                if (triggerAction == null)
                                {
                                    return;
                                }

                                triggerAction.Invoke(hit);
                                _centerDoubleClickRay.Remove(hit.collider.gameObject);
                            }
                            else
                            {
                                _centerDoubleClickRay.Add(hit.collider.gameObject);
                                TimeFrameComponent.Instance.AddTimeTask(() =>
                                {
                                    if (_centerDoubleClickRay.Contains(hit.collider.gameObject))
                                    {
                                        _centerDoubleClickRay.Remove(hit.collider.gameObject);
                                    }
                                }, "左键双击移除", doubleClickTime);
                            }

                            break;
                        case MouseType.Right:
                            if (_rightDoubleClickRay.Contains(hit.collider.gameObject))
                            {
                                if (triggerAction == null)
                                {
                                    return;
                                }

                                triggerAction.Invoke(hit);
                                _rightDoubleClickRay.Remove(hit.collider.gameObject);
                            }
                            else
                            {
                                _rightDoubleClickRay.Add(hit.collider.gameObject);
                                TimeFrameComponent.Instance.AddTimeTask(() =>
                                {
                                    if (_rightDoubleClickRay.Contains(hit.collider.gameObject))
                                    {
                                        _rightDoubleClickRay.Remove(hit.collider.gameObject);
                                    }
                                }, "左键双击移除", doubleClickTime);
                            }

                            break;
                    }
                }
                else if (triggerType == TriggerType.Down)
                {
                    if (triggerAction == null)
                    {
                        return;
                    }

                    triggerAction.Invoke(hit);
                }
                else if (triggerType == TriggerType.Up)
                {
                    if (triggerAction == null)
                    {
                        return;
                    }

                    triggerAction.Invoke(hit);
                }
                else if (triggerType == TriggerType.Normal)
                {
                    if (triggerAction == null)
                    {
                        return;
                    }

                    triggerAction.Invoke(hit);
                }
            }
            else
            {
                //离开触发区域
                if (unTriggerAction == null)
                {
                    return;
                }

                unTriggerAction.Invoke(hit);
            }
        }

        public void CloseRayTest(int timeTask)
        {
            for (int i = 0; i < rayRenderItemInfos.Count; i++)
            {
                if (rayRenderItemInfos[i].actionId == timeTask)
                {
                    rayRenderItemInfos.RemoveAt(i);
                    break;
                }
            }
        }
    }
}