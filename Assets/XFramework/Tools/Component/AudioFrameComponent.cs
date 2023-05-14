using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 音乐组件
    /// </summary>
    public class AudioFrameComponent : FrameComponent
    {
        public static AudioFrameComponent Instance;
        private AudioSource _backgroundAudioSource;
        private AudioSource _effectAudioSource;
        private AudioSource _tipAndDialogAudioSource;

        [Searchable] [TableList(AlwaysExpanded = true)] [InlineEditor()] [Required("选择音频配置文件")] [LabelText("音频数据")]
        public AudioComponentData audioData;

        private Dictionary<string, AudioClip> _audioDlc;

        public override void FrameInitComponent()
        {
            Instance = GetComponent<AudioFrameComponent>();
            //创建音效组件
            if (_effectAudioSource == null)
            {
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = false;
            }

            //创建提示与对话组件
            if (_tipAndDialogAudioSource == null)
            {
                _tipAndDialogAudioSource = gameObject.AddComponent<AudioSource>();
                _tipAndDialogAudioSource.playOnAwake = false;
            }

            //创建背景音乐组件
            if (_backgroundAudioSource == null)
            {
                _backgroundAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = true;
                _backgroundAudioSource.volume = 0.5f;
                _backgroundAudioSource.loop = true;
            }

            //音效初始化
            _audioDlc = new Dictionary<string, AudioClip>();
            if (audioData != null)
            {
                foreach (AudioComponentData.AudioInfo audioDataInfo in audioData.audioInfos)
                {
                    if (!_audioDlc.ContainsKey(audioDataInfo.audioName) && audioDataInfo.audioClip != null)
                    {
                        _audioDlc.Add(audioDataInfo.audioName, audioDataInfo.audioClip);
                    }
                }
            }

            //播放背景音乐
            PlayBackgroundAudio();
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }


        [InfoBox("播放音效")]
        public void PlayEffectAudio(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                _effectAudioSource.volume = 1;
                _effectAudioSource.clip = _audioDlc[audioName];
                _effectAudioSource.Play();
            }
        }

        public float GetEffectAudioLength(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                if (_effectAudioSource.clip != null)
                {
                    return _effectAudioSource.clip.length;
                }
            }

            return -1;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayEffectAudio(AudioClip audioClip)
        {
            _effectAudioSource.clip = audioClip;
            _effectAudioSource.Play();
        }

        public void PlayTipAndDialogAudio(AudioClip audioClip)
        {
            _tipAndDialogAudioSource.Stop();
            _tipAndDialogAudioSource.clip = audioClip;
            _tipAndDialogAudioSource.Play();
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopEffectAudio()
        {
            _effectAudioSource.Stop();
            _effectAudioSource.clip = null;
        }

        public void SetEffectVolume(float volume)
        {
            _effectAudioSource.volume = volume;
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopTipAndDialogAudio()
        {
            _tipAndDialogAudioSource.Stop();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _effectAudioSource.Pause();
            _backgroundAudioSource.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            _effectAudioSource.UnPause();
            if (RuntimeDataFrameComponent.Instance.audioState)
            {
                _backgroundAudioSource.UnPause();
            }
        }


        /// <summary>
        /// 切换音乐状态
        /// </summary>
        public void SwitchBackgroundState()
        {
            if (_backgroundAudioSource.isPlaying)
            {
                PauseBackgroundAudio();
            }
            else
            {
                PlayBackgroundAudio();
            }
        }

        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void PauseBackgroundAudio()
        {
            PlayEffectAudio("背景音乐");
            _backgroundAudioSource.Pause();
            RuntimeDataFrameComponent.Instance.audioState = false;
        }

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio()
        {
            if (_audioDlc.ContainsKey("背景音乐") && _audioDlc["背景音乐"] != null)
            {
                _backgroundAudioSource.clip = _audioDlc["背景音乐"];
                _backgroundAudioSource.Play();
                RuntimeDataFrameComponent.Instance.audioState = true;
            }
            else
            {
                // Debug.LogError("没有指定背景音乐");
            }
        }
    }
}