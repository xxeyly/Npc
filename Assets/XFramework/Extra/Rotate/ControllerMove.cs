using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class ControllerMove : MonoBehaviour
    {
        [BoxGroup("基础")] [LabelText("开启")] public bool playing = true;
        [BoxGroup("基础")] [LabelText("目标物体")] public Transform target;
        [BoxGroup("基础")] [LabelText("目标相机")] public Camera targetCamera;

        #region 移动

        [BoxGroup("移动")] [LabelText("是否可移动")] public bool canManualMove = true;
        [BoxGroup("移动")] [LabelText("移动反转")] public bool moveReversal = true;
        [BoxGroup("移动")] [LabelText("移动按键")] public KeyCode moveCode;
        [BoxGroup("移动")] [LabelText("移动速度")] public float movSpeed = 0.005f;
        [BoxGroup("移动")] [LabelText("移动增量")] public Vector3 movingIncrement;

        [BoxGroup("移动")] [LabelText("本地X限定")] public Vector2 xTargetLimit;

        [BoxGroup("移动")] [LabelText("本地Y限定")] public Vector2 yTargetLimit;

        [BoxGroup("移动")] [LabelText("本地Z限定")] public Vector2 zTargetLimit;

        [BoxGroup("移动")] [LabelText("X轴前限制")] public bool selfLeftDirectionLimit;
        [BoxGroup("移动")] [LabelText("X轴后限制")] public bool selfRightDirectionLimit;
        [BoxGroup("移动")] [LabelText("Y轴前限制")] public bool selfUpDirectionLimit;
        [BoxGroup("移动")] [LabelText("Y轴后限制")] public bool selfDownDirectionLimit;
        [BoxGroup("移动")] [LabelText("Z轴前限制")] public bool selfForwardDirectionLimit;
        [BoxGroup("移动")] [LabelText("Z轴后限制")] public bool selfBackDirectionLimit;
        private Vector3 _limitValue;
        float _yDis, _xDis;
        private bool _firstMove;
        private Vector3 _oldMousePos;
        private Vector3 _currentMousePos;
        private Vector3 _oldTargetPos;

        #endregion

        private void Update()
        {
            if (playing)
            {
                Move();
            }
        }

        private void Move()
        {
            if (Input.GetKey(moveCode) && canManualMove)
            {
                _limitValue = target.localPosition;
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
                    Vector3 cameraValue = DataFrameComponent.GetInspectorEuler(targetCamera.transform);
                    Quaternion rot = Quaternion.Euler(cameraValue.x, cameraValue.y, 0);
                    if (moveReversal)
                    {
                        _xDis = -_xDis;
                        _yDis = -_yDis;
                    }

                    //目标世界坐标
                    Vector3 targetWorldPos = rot * new Vector3(_xDis, _yDis, 0) + target.position;
                    //目标局部坐标
                    Vector3 targetSelfPos;
                    //不包含父物体
                    if (target.parent == null)
                    {
                        //世界坐标等于局部坐标
                        targetSelfPos = targetWorldPos;
                    }
                    else
                    {
                        //将世界坐标转换局部坐标
                        targetSelfPos = target.parent.InverseTransformPoint(targetWorldPos);
                    }

                    if (targetSelfPos.x < _limitValue.x && selfRightDirectionLimit)
                    {
                        targetSelfPos = new Vector3(_limitValue.x, targetSelfPos.y, targetSelfPos.z);
                    }

                    if (targetSelfPos.x > _limitValue.x && selfLeftDirectionLimit)
                    {
                        targetSelfPos = new Vector3(_limitValue.x, targetSelfPos.y, targetSelfPos.z);
                    }

                    if (targetSelfPos.y < _limitValue.y && selfDownDirectionLimit)
                    {
                        targetSelfPos = new Vector3(targetSelfPos.x, _limitValue.y, targetSelfPos.z);
                    }

                    if (targetSelfPos.y > _limitValue.y && selfUpDirectionLimit)
                    {
                        targetSelfPos = new Vector3(targetSelfPos.x, _limitValue.y, targetSelfPos.z);
                    }

                    if (targetSelfPos.z < _limitValue.z && selfBackDirectionLimit)
                    {
                        targetSelfPos = new Vector3(targetSelfPos.x, targetSelfPos.y, _limitValue.z);
                    }

                    if (targetSelfPos.z > _limitValue.z && selfForwardDirectionLimit)
                    {
                        targetSelfPos = new Vector3(targetSelfPos.x, targetSelfPos.y, _limitValue.z);
                    }

                    target.localPosition = targetSelfPos;
                    movingIncrement = targetSelfPos - _oldTargetPos;
                    _oldTargetPos = targetSelfPos;
                }
                else
                {
                    movingIncrement = Vector3.zero;
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
    }
}