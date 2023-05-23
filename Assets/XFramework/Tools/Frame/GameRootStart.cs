using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    [InfoBox("框架开始")]
    public class GameRootStart : MonoBehaviour
    {
        public static GameRootStart Instance;
#pragma warning disable 649
        [LabelText("框架组件")] [Searchable] public List<FrameComponent> frameComponent = new List<FrameComponent>();
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool initJump;

        [BoxGroup("场景加载")] [LabelText("初始化跳转场景名称")]
        public string initJumpSceneName;

        [BoxGroup("场景加载")] [LabelText("场景")] public Scene loadScene;

        [BoxGroup("框架场景组件-不摧毁")] [LabelText("框架场景组件")] [Searchable]
        public List<SceneComponent> dontDestroyFrameSceneComponents = new List<SceneComponent>();

        [BoxGroup("框架场景组件-不摧毁")] [LabelText("框架场景初始化组件")] [Searchable]
        public List<SceneComponentInit> frameSceneInitStartSingletons = new List<SceneComponentInit>();

        [BoxGroup("实时场景组件")] [LabelText("场景组件")] [Searchable]
        public List<SceneComponent> sceneComponents = new List<SceneComponent>();

        [BoxGroup("实时场景组件")] [LabelText("场景初始化组件")] [Searchable]
        public List<SceneComponentInit> sceneInitStartSingletons = new List<SceneComponentInit>();

        [LabelText("热更加载")] [BoxGroup] public bool hotFixLoad;
        [LabelText("禁止摧毁")] [BoxGroup] public bool dontDestroyOnLoad;

        [LabelText("热修复包AssetBundle配置")] public List<HotFixAssetAssetBundleSceneConfig> hotFixAssetAssetBundleSceneConfigs = new List<HotFixAssetAssetBundleSceneConfig>();
        [LabelText("框架加载日志")] [BoxGroup] public bool frameLoadLog;

        private List<AssetBundle> _currentSceneAllAssetBundle = new List<AssetBundle>();


        private void OnEnable()
        {
            //场景中只有一个GameRootStart
            if (DataFrameComponent.GetAllObjectsInScene<GameRootStart>().Count == 1)
            {
                if (DataFrameComponent.GetAllObjectsInScene<GameRootStart>()[0].dontDestroyOnLoad)
                {
                    return;
                }
            }
            //多余GameRootStart销毁
            else
            {
                foreach (GameRootStart gameRootStart in DataFrameComponent.GetAllObjectsInScene<GameRootStart>())
                {
                    if (!gameRootStart.dontDestroyOnLoad)
                    {
                        DestroyImmediate(gameRootStart.gameObject);
                    }
                }

                return;
            }

            DontDestroyOnLoad(this);
            dontDestroyOnLoad = true;
            Instance = GetComponent<GameRootStart>();
            Debug.Log(gameObject.scene.name);
            Debug.Log("框架初始化");
            frameComponent = DataFrameComponent.GetAllObjectsInScene<FrameComponent>("DontDestroyOnLoad");
            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameInitComponent();
            }

            if (hotFixLoad)
            {
                string hotFixAssetConfig = FileOperation.GetTextToLoad(Application.streamingAssetsPath, "HotFixAssetConfig.json");
                hotFixAssetAssetBundleSceneConfigs = JsonMapper.ToObject<List<HotFixAssetAssetBundleSceneConfig>>(hotFixAssetConfig);
            }

            Debug.Log("框架初始化完毕");
            //框架组件开启

            dontDestroyFrameSceneComponents = DataFrameComponent.GetAllObjectsInScene<SceneComponent>("DontDestroyOnLoad");
            for (int i = 0; i < dontDestroyFrameSceneComponents.Count; i++)
            {
                dontDestroyFrameSceneComponents[i].StartComponent();
            }

            Debug.Log("不摧毁的SceneComponent加载完毕");

            frameSceneInitStartSingletons = DataFrameComponent.GetAllObjectsInScene<SceneComponentInit>("DontDestroyOnLoad");
            for (int i = 0; i < frameSceneInitStartSingletons.Count; i++)
            {
                frameSceneInitStartSingletons[i].InitComponent();
            }

            Debug.Log("不摧毁的SceneComponentInit加载完毕");

            if (initJump)
            {
                Debug.Log("初始场景跳转");
                SceneLoadFrameComponent.Instance.SceneLoad(initJumpSceneName);
                DestroyImmediate(GetComponent<AudioListener>());
            }

            SceneManager.sceneLoaded += SceneLoadOverCallBack;
        }

        public void SceneAssetBundleUnload()
        {
            if (!hotFixLoad)
            {
                return;
            }

            foreach (AssetBundle assetBundle in _currentSceneAllAssetBundle)
            {
                if (assetBundle != null)
                {
                    assetBundle.Unload(false);
                }
            }
        }

        /// <summary>
        /// 场景加载完毕回调
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneType"></param>
        private void SceneLoadOverCallBack(Scene scene, LoadSceneMode sceneType)
        {
            loadScene = scene;
            InitSceneStartSingletons(scene);
        }

        /// <summary>
        /// 加载场景初始化单例
        /// 加载顺序 框架组件-场景工具
        /// </summary>
        private void InitSceneStartSingletons(Scene scene)
        {
            InstantiateHotFixAssetBundle();
            Debug.Log(scene.name + "场景加载完毕");
            FrameComponentSceneInit();
            Debug.Log(scene.name + "框架场景初始化");
            SceneComponentStart(scene);
            SceneComponentInitStart(scene);
            Debug.Log(scene.name + ":" + "场景初始化完毕");
        }

        #region 热更

        private void InstantiateHotFixAssetBundle()
        {
            if (!Instance.hotFixLoad)
            {
                return;
            }

            //获得指定场景内容
            HotFixAssetAssetBundleSceneConfig currentSceneHotFixAssetAssetBundleSceneConfig = null;
            foreach (HotFixAssetAssetBundleSceneConfig hotFixAssetAssetBundleSceneConfig in hotFixAssetAssetBundleSceneConfigs)
            {
                if (hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.assetBundleName == DataFrameComponent.AllCharToLower(loadScene.name))
                {
                    currentSceneHotFixAssetAssetBundleSceneConfig = hotFixAssetAssetBundleSceneConfig;
                    break;
                }
            }
            //加载内容

            for (int i = 0; i < currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                AssetBundle tempHotFixAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundlePath);
                _currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
                GameObject hotFixObject = tempHotFixAssetBundle.LoadAsset<GameObject>(currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);
                switch (currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleType)
                {
                    case HotFixAssetType.UI:
                        Instantiate(hotFixObject, GameObject.Find(currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath).transform, false);
                        break;
                    case HotFixAssetType.Scene:
                        break;
                    case HotFixAssetType.Env:
                    case HotFixAssetType.Entity:
                        if (currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath == string.Empty)
                        {
                            Instantiate(hotFixObject, null, false);
                        }
                        else
                        {
                            Instantiate(hotFixObject, GameObject.Find(currentSceneHotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath).transform, false);
                        }

                        break;
                    case HotFixAssetType.SceneComponent:
                        Instantiate(hotFixObject, null, false);
                        break;
                    case HotFixAssetType.SceneComponentInit:
                        Instantiate(hotFixObject, null, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        //场景加载前准备
        public void SceneBeforeLoadPrepare(string destroySceneName)
        {
            Debug.Log(destroySceneName);
            //视图摧毁
            ViewFrameComponent.Instance.AllViewDestroy(destroySceneName);
            //计时器初始化
            TimeFrameComponent.Instance.FrameSceneInitComponent();
            //实体清除
            EntityFrameComponent.Instance.RemoveSceneEntityName();
            //场景组件移除
            List<SceneComponent> tempSceneComponent = DataFrameComponent.GetAllObjectsInScene<SceneComponent>(destroySceneName);
            foreach (SceneComponent sceneComponent in tempSceneComponent)
            {
                if (sceneComponents.Contains(sceneComponent))
                {
                    sceneComponent.EndComponent();
                    sceneComponents.Remove(sceneComponent);
                }
            }

            //场景初始化组件移除
            List<SceneComponentInit> tempSceneComponentInit = DataFrameComponent.GetAllObjectsInScene<SceneComponentInit>(destroySceneName);
            foreach (SceneComponentInit sceneComponentInit in tempSceneComponentInit)
            {
                if (sceneInitStartSingletons.Contains(sceneComponentInit))
                {
                    sceneInitStartSingletons.Remove(sceneComponentInit);
                }
            }
        }


        private void OnDestroy()
        {
            if (dontDestroyOnLoad)
            {
                foreach (FrameComponent componentBase in frameComponent)
                {
                    componentBase.FrameEndComponent();
                }

                SceneComponentEnd();
            }
        }


        [LabelText("开启框架组件")]
        private void FrameComponentInit()
        {
            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameInitComponent();
            }
        }

        [LabelText("框架组件场景初始化")]
        private void FrameComponentSceneInit()
        {
            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameSceneInitComponent();
            }
        }

        [LabelText("开启场景组件")]
        public void SceneComponentStart(Scene scene)
        {
            sceneComponents = DataFrameComponent.GetAllObjectsInScene<SceneComponent>(scene.name);
            for (int i = 0; i < sceneComponents.Count; i++)
            {
                sceneComponents[i].StartComponent();
            }
        }

        [LabelText("开启场景初始化组件")]
        public void SceneComponentInitStart(Scene scene)
        {
            sceneInitStartSingletons = DataFrameComponent.GetAllObjectsInScene<SceneComponentInit>(scene.name);
            for (int i = 0; i < sceneInitStartSingletons.Count; i++)
            {
                sceneInitStartSingletons[i].InitComponent();
            }
        }

        //场景组件结束
        [LabelText("结束场景组件")]
        private void SceneComponentEnd()
        {
            List<SceneComponent> tempSceneComponent = DataFrameComponent.GetAllObjectsInScene<SceneComponent>();
            for (int i = 0; i < tempSceneComponent.Count; i++)
            {
                tempSceneComponent[i].EndComponent();
            }
        }
    }
}