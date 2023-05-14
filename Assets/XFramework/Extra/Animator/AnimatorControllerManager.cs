using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using XFramework;

namespace XFramework
{
    /// <summary>
    /// 动画控制管理器
    /// </summary>
    public class AnimatorControllerManager : SceneComponent
    {
        public static AnimatorControllerManager Instance;
        [LabelText("当前播放动画名称")] public string currentPlayAnim;
        [LabelText("动画是否切换")] public bool eventChange;

        [LabelText("所有动画控制器")] [SerializeField] [Searchable]
        public List<AnimatorControllerBase> allAnimController = new List<AnimatorControllerBase>();

        /// <summary>
        /// 动画控制器任务
        /// </summary>
        private int _animatorTimeTask;

        public override void StartComponent()
        {
            Instance = GetComponent<AnimatorControllerManager>();
            allAnimController = DataFrameComponent.GetAllObjectsInScene<AnimatorControllerBase>();
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
            {
                animatorControllerBase.Init();
            }
        }

        public override void EndComponent()
        {
        }

        [LabelText("添加动画控制器")]
        public void AddAnimatorController(AnimatorControllerBase animatorControllerBase)
        {
            if (!allAnimController.Contains(animatorControllerBase))
            {
                allAnimController.Add(animatorControllerBase);
            }
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        public void PlayAnim(string animType)
        {
            currentPlayAnim = animType;
            eventChange = true;
            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.gameObject.activeInHierarchy)
                {
                    controllerBase.PlayAnim(animType);
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="playProgress">播放进度</param>
        public void PlayAnim(string animType, float playProgress)
        {
            currentPlayAnim = animType;
            eventChange = true;

            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.enabled)
                {
                    controllerBase.PlayAnim(animType, playProgress);
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animSpeedProgress"></param>
        public void PlayAnim(string animType, AnimSpeedProgress animSpeedProgress)
        {
            currentPlayAnim = animType;
            eventChange = true;

            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                try
                {
                    if (controllerBase.gameObject.activeInHierarchy)
                    {
                        controllerBase.PlayAnim(animType, animSpeedProgress);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(controllerBase.name + e);
                    throw;
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animAction"></param>
        public int PlayAnim(string animType, UnityAction animAction)
        {
            currentPlayAnim = animType;
            eventChange = false;

            TimeFrameComponent.Instance.DeleteTimeTask(_animatorTimeTask);
            AnimatorControllerBase fistController = GetPlayAnimFirstController(animType);

            _animatorTimeTask = TimeFrameComponent.Instance.AddTimeTask(() =>
                {
                    if (fistController == null)
                    {
                        return;
                    }

                    if (fistController.GetAnimClipPlayOver())
                    {
                        animAction.Invoke();
                    }
                }
                , "动画播放时间", GetPlayAnimFirstLength(animType));
            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }

            return _animatorTimeTask;
        }

        public void StopAnimAction()
        {
            TimeFrameComponent.Instance.DeleteTimeTask(_animatorTimeTask);
            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnim();
            }
        }

        public void StopAllAnimAction()
        {
            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnimTaskTime();
            }
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimFirstLength(string animType)
        {
            // float animLength = 0;
            AnimatorControllerBase fistController = GetPlayAnimFirstController(animType);
            if (fistController != null)
            {
                return fistController.GetPlayAnimLength(animType);
            }

            return 0;
        }

        /// <summary>
        /// 获得首个能播放片段的控制器
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public AnimatorControllerBase GetPlayAnimFirstController(string animType)
        {
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (animatorControllerBase.GetAnimState(animType) && animatorControllerBase.gameObject.activeInHierarchy)
                {
                    return animatorControllerBase;
                }
            }

            return null;
        }

        /// <summary>
        /// 获得动画状态
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public bool GetAnimState(string animType)
        {
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (animatorControllerBase.GetAnimState(animType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void PausePlay()
        {
            foreach (AnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.PausePlay();
            }
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            foreach (AnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.ContinuePlay();
            }
        }
    }
}