using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework
{
    /// <summary>
    /// 播放动画进化
    /// </summary>
    public enum AnimSpeedProgress
    {
        None,
        Start,
        End
    }

    /// <summary>
    /// 动画控制器基类
    /// </summary>
    public abstract class AnimatorControllerBase : MonoBehaviour
    {
        [BoxGroup("初始化")] [SerializeField] [LabelText("视图是否初始化")]
        public bool isInit = false;

        private Animator _animator;
        [LabelText("动画属性")] [SerializeField] private List<string> animatorControllerParameterName;
        [SerializeField] private List<AnimatorControllerParameter> _allParameter = new List<AnimatorControllerParameter>();
        [LabelText("动画初始化")] public bool isLoadParameter;


        public void AddToAnimatorControllerList()
        {
            if (AnimatorControllerManager.Instance != null)
            {
                if (!AnimatorControllerManager.Instance.allAnimController.Contains(this))
                {
                    AnimatorControllerManager.Instance.allAnimController.Add(this);
                    if (!isInit)
                    {
                        Init();
                    }
                }
            }
        }

        public void Init()
        {
            _animator = GetComponent<Animator>();
            animatorControllerParameterName = new List<string>();
            if (gameObject.activeInHierarchy)
            {
                _allParameter = new List<AnimatorControllerParameter>(_animator.parameters);
                foreach (AnimatorControllerParameter animatorControllerParameter in _allParameter)
                {
                    animatorControllerParameterName.Add(animatorControllerParameter.name);
                }

                isLoadParameter = true;
            }

            isInit = true;
        }

        private bool ContainsParameter(string parameterName)
        {
            if (!gameObject.activeInHierarchy)
            {
                return false;
            }

            if (!isLoadParameter)
            {
                _allParameter = new List<AnimatorControllerParameter>(_animator.parameters);
                foreach (AnimatorControllerParameter animatorControllerParameter in _allParameter)
                {
                    animatorControllerParameterName.Add(animatorControllerParameter.name);
                }

                isLoadParameter = true;
            }

            if (isLoadParameter)
            {
                foreach (AnimatorControllerParameter animatorControllerParameter in _allParameter)
                {
                    if (animatorControllerParameter.name == parameterName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void PausePlay()
        {
            _animator.speed = 0;
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            _animator.speed = 1;
        }


        public void PlayAnim(string animationType, float animValue)
        {
            if (ContainsParameter(animationType))
            {
                _animator.speed = 0;
                if (animValue >= 1f)
                {
                    animValue = 1f;
                }

                _animator.Play(animationType, 0, animValue);
            }
        }

        public void PlayAnim(string animationType, AnimSpeedProgress animSpeedProgress)
        {
            if (ContainsParameter(animationType))
            {
                _animator.speed = 0;
                if (animSpeedProgress == AnimSpeedProgress.End)
                {
                    _animator.Play(animationType, 0, 1f);
                }
                else if (animSpeedProgress == AnimSpeedProgress.Start)
                {
                    _animator.Play(animationType, 0, normalizedTime: 0f);
                }
            }
        }

        private int _playAnimTimeTask;

        /// <summary>
        /// 播放动画+事件
        /// </summary>
        /// <param name="animationType"></param>
        /// <param name="eventAction"></param>
        public int PlayAnim(string animationType, UnityAction eventAction)
        {
            if (ContainsParameter(animationType))
            {
                PlayAnim(animationType);
                return _playAnimTimeTask = TimeFrameComponent.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType));
            }
            else
            {
                Debug.Log("不包含当前动画:" + animationType);
            }

            return 0;
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animationType"></param>
        public void PlayAnim(string animationType)
        {
            if (ContainsParameter(animationType))
            {
                _animator.speed = 1;
                _animator.SetTrigger(animationType);
            }
        }

        /// <summary>
        /// 获得当前动画播放是否完毕
        /// </summary>
        /// <returns></returns>
        public bool GetAnimClipPlayOver()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f && _animator.gameObject.activeSelf &&
                   !AnimatorControllerManager.Instance.eventChange;
        }

        /// <summary>
        /// 停止播放动画
        /// </summary>
        public void StopAnim()
        {
            _animator.speed = 0;
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(string animType)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType)
                {
                    return item.length / GetPlayAnimPlaySpeed();
                }
            }

            return -1;
        }

        public float GetPlayAnimPlaySpeed()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).speed;
        }

        /// <summary>
        /// 获得动画状态
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public bool GetAnimState(string animType)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType)
                {
                    return true;
                }
            }

            return false;
        }

        public void StopAnimTaskTime()
        {
            StopAnim();
            TimeFrameComponent.Instance.DeleteTimeTask(_playAnimTimeTask);
        }
    }
}