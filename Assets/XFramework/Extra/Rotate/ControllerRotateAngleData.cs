using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerRotateAngleData : MonoBehaviour
    {
        [LabelText("左右角度限定")] public Vector2 leftAndRightLimit;

        [LabelText("上下角度限定")] public Vector2 topAndDownLimit;
    }
}