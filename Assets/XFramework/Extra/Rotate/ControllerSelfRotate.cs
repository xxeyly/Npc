using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class ControllerSelfRotate : MonoBehaviour
    {
        public enum Axial
        {
            X,
            Y,
            Z
        }

        // Start is called before the first frame update
        [LabelText("是否开启旋转")] public bool isOpenMouseOperation;
        [LabelText("开启水平轴向")] public bool isHorizontal;
        [LabelText("开启垂直轴向")] public bool isVertical;
        [LabelText("目标物体")] [SerializeField] public Transform targetTri;
        private Quaternion _targetRotation;
        [LabelText("旋转速度")] public float rotateSpeed = 3;
        [LabelText("平滑过渡")] public bool smoothTransition;
        [LabelText("水平值")] public float mouseX;
        [LabelText("垂直值")] public float mouseY;
        [LabelText("水平反转")] public bool hReversal;
        [LabelText("垂直反转")] public bool vReversal;

        [LabelText("左右角度限定")] public Vector2 leftAndRightLimit;

        [LabelText("上下角度限定")] public Vector2 topAndDownLimit;

        [LabelText("水平旋转轴向")] public Axial mouseXAxial;
        [LabelText("垂直旋转轴向")] public Axial mouseYAxial;

        [SerializeField] [LabelText("默认角度")] Vector3 defaultEuler = Vector3.zero;
        [SerializeField] private float inputX;
        [SerializeField] private float inputY;
        float _velocity = 0.0f;

        public void SetRotateObj()
        {
            if (targetTri != null)
            {
                SetRotateObj(targetTri);
            }
        }

        public void SetRotateObj(Transform target)
        {
            defaultEuler = GetInspectorEuler(target);

            targetTri = target;
            switch (mouseXAxial)
            {
                case Axial.X:
                    if (leftAndRightLimit != Vector2.zero)
                    {
                        mouseX = ClampAngle(defaultEuler.x, leftAndRightLimit.x, leftAndRightLimit.y);
                    }
                    else
                    {
                        mouseX = defaultEuler.x;
                    }

                    break;
                case Axial.Y:
                    if (leftAndRightLimit != Vector2.zero)
                    {
                        mouseX = ClampAngle(defaultEuler.y, leftAndRightLimit.x, leftAndRightLimit.y);
                    }
                    else
                    {
                        mouseX = defaultEuler.y;
                    }

                    break;
                case Axial.Z:
                    if (leftAndRightLimit != Vector2.zero)
                    {
                        mouseX = ClampAngle(defaultEuler.z, leftAndRightLimit.x, leftAndRightLimit.y);
                    }
                    else
                    {
                        mouseX = defaultEuler.z;
                    }

                    break;
            }

            switch (mouseYAxial)
            {
                case Axial.X:
                    if (topAndDownLimit != Vector2.zero)
                    {
                        mouseY = ClampAngle(defaultEuler.x, topAndDownLimit.x, topAndDownLimit.y);
                    }
                    else
                    {
                        mouseY = defaultEuler.x;
                    }

                    break;
                case Axial.Y:
                    if (topAndDownLimit != Vector2.zero)
                    {
                        mouseY = ClampAngle(defaultEuler.y, topAndDownLimit.x, topAndDownLimit.y);
                    }
                    else
                    {
                        mouseY = defaultEuler.y;
                    }

                    break;
                case Axial.Z:
                    if (topAndDownLimit != Vector2.zero)
                    {
                        mouseY = ClampAngle(defaultEuler.z, topAndDownLimit.x, topAndDownLimit.y);
                    }
                    else
                    {
                        mouseY = defaultEuler.z;
                    }

                    break;
            }
        }

        private void Update()
        {
            #region 控制旋转数值

            if (isOpenMouseOperation)
            {
                if (isHorizontal)
                {
                    inputX = Input.GetAxis("Mouse X");
                }
                else
                {
                    inputX = 0;
                }

                if (isVertical)
                {
                    inputY = Input.GetAxis("Mouse Y");
                }
                else
                {
                    inputY = 0;
                }
            }
            else
            {
                inputX = Mathf.SmoothDamp(inputX, 0, ref _velocity, Time.deltaTime);
                inputY = Mathf.SmoothDamp(inputY, 0, ref _velocity, Time.deltaTime);
            }

            #endregion

            if (hReversal)
            {
                mouseX -= -inputX * rotateSpeed;
            }
            else
            {
                mouseX += -inputX * rotateSpeed;
            }

            if (vReversal)
            {
                mouseY -= inputY * rotateSpeed;
            }
            else
            {
                mouseY += inputY * rotateSpeed;
            }


            if (leftAndRightLimit != Vector2.zero)
            {
                mouseX = ClampAngle(mouseX, leftAndRightLimit.x, leftAndRightLimit.y);
            }

            if (topAndDownLimit != Vector2.zero)
            {
                mouseY = ClampAngle(mouseY, topAndDownLimit.x, topAndDownLimit.y);
            }


            Vector3 nextPos = defaultEuler;

            if (isHorizontal)
            {
                switch (mouseXAxial)
                {
                    case Axial.X:

                        nextPos.x = mouseX;
                        defaultEuler.x = mouseX;
                        if (isVertical)
                        {
                            switch (mouseYAxial)
                            {
                                case Axial.X:
                                    nextPos.y = defaultEuler.y;
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Y:
                                    nextPos.z = defaultEuler.z;

                                    break;
                                case Axial.Z:
                                    nextPos.y = defaultEuler.y;

                                    break;
                            }
                        }
                        else
                        {
                            nextPos.y = defaultEuler.y;
                            nextPos.z = defaultEuler.z;
                        }


                        break;
                    case Axial.Y:
                        nextPos.y = mouseX;
                        defaultEuler.y = mouseX;

                        if (isVertical)
                        {
                            switch (mouseYAxial)
                            {
                                case Axial.X:
                                    nextPos.z = defaultEuler.z;

                                    break;
                                case Axial.Y:
                                    nextPos.x = defaultEuler.x;
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Z:
                                    nextPos.x = defaultEuler.x;

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            nextPos.x = defaultEuler.x;
                            nextPos.z = defaultEuler.z;
                        }

                        break;
                    case Axial.Z:
                        nextPos.z = mouseX;
                        defaultEuler.z = mouseX;

                        if (isVertical)
                        {
                            switch (mouseYAxial)
                            {
                                case Axial.X:
                                    nextPos.y = defaultEuler.y;

                                    break;
                                case Axial.Y:
                                    nextPos.x = defaultEuler.x;

                                    break;
                                case Axial.Z:
                                    nextPos.x = defaultEuler.x;
                                    nextPos.y = defaultEuler.y;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            nextPos.x = defaultEuler.x;
                            nextPos.y = defaultEuler.y;
                        }

                        break;
                }
            }

            if (isVertical)
            {
                switch (mouseYAxial)
                {
                    case Axial.X:
                        nextPos.x = mouseY;
                        defaultEuler.x = mouseY;

                        if (isHorizontal)
                        {
                            switch (mouseXAxial)
                            {
                                case Axial.X:
                                    nextPos.y = defaultEuler.y;
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Y:
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Z:
                                    nextPos.y = defaultEuler.y;

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            nextPos.y = defaultEuler.y;
                            nextPos.z = defaultEuler.z;
                        }

                        break;
                    case Axial.Y:
                        nextPos.y = mouseY;
                        defaultEuler.y = mouseY;

                        if (isHorizontal)
                        {
                            switch (mouseXAxial)
                            {
                                case Axial.X:
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Y:
                                    nextPos.x = defaultEuler.x;
                                    nextPos.z = defaultEuler.z;
                                    break;
                                case Axial.Z:
                                    nextPos.x = defaultEuler.x;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            nextPos.x = defaultEuler.x;
                            nextPos.z = defaultEuler.z;
                        }

                        break;
                    case Axial.Z:

                        nextPos.z = mouseY;
                        defaultEuler.z = mouseY;

                        if (isHorizontal)
                        {
                            switch (mouseXAxial)
                            {
                                case Axial.X:
                                    nextPos.y = defaultEuler.y;

                                    break;
                                case Axial.Y:
                                    nextPos.x = defaultEuler.x;
                                    break;
                                case Axial.Z:
                                    nextPos.x = defaultEuler.x;
                                    nextPos.y = defaultEuler.y;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            nextPos.x = defaultEuler.x;
                            nextPos.y = defaultEuler.y;
                        }

                        break;
                }
            }

            if (smoothTransition)
            {
                _targetRotation = Quaternion.Euler(nextPos);
                targetTri.rotation = Quaternion.Lerp(targetTri.rotation, _targetRotation,
                    Time.deltaTime * rotateSpeed);
            }
            else
            {
                targetTri.localEulerAngles = nextPos;
            }
        }

        //function used to limit angles
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360F) && (angle <= 360F))
            {
                if (angle < -360F)
                {
                    angle += 360F;
                }

                if (angle > 360F)
                {
                    angle -= 360F;
                }
            }

            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        /// 获取面板上的值
        /// </summary>
        /// <param name="mTransform"></param>
        /// <returns></returns>
        private Vector3 GetInspectorEuler(Transform mTransform)
        {
            Vector3 angle = mTransform.localEulerAngles;
            float x = angle.x;
            float y = angle.y;
            float z = angle.z;

            if (Vector3.Dot(mTransform.up, Vector3.up) >= 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = angle.x;
                }

                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = angle.x - 360f;
                }
            }

            if (Vector3.Dot(mTransform.up, Vector3.up) < 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = 180 - angle.x;
                }

                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = 180 - angle.x;
                }
            }

            if (angle.y > 180)
            {
                y = angle.y - 360f;
            }

            if (angle.z > 180)
            {
                z = angle.z - 360f;
            }

            Vector3 vector3 = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
            return vector3;
        }

        /// <summary>
        /// 设置限定角度
        /// </summary>
        /// <param name="leftAndRight"></param>
        /// <param name="tpoAndDown"></param>
        public void SetRotateAngleLimit(Vector2 leftAndRight, Vector2 tpoAndDown)
        {
            leftAndRightLimit = leftAndRight;
            topAndDownLimit = tpoAndDown;
        }

        /// <summary>
        /// 设置限定角度
        /// </summary>
        /// <param name="controllerRotateAngleData"></param>
        public void SetRotateAngleLimit(ControllerRotateAngleData controllerRotateAngleData)
        {
            SetRotateAngleLimit(controllerRotateAngleData.leftAndRightLimit, controllerRotateAngleData.topAndDownLimit);
        }
    }
}